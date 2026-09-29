using AutoMapper;
using Tourism.Core.Features.Users.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tourism.Core.Entities;
using Tourism.Core.Enum;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.Authentication.DTOs;
using Tourism.Core.Features.UploadImage.Interfaces;
using Tourism.Core.Features.Users.DTOs;
using Tourism.Core.Features.Users.Interfaces;
using Tourism.Repository.Data.DBContext;

namespace Tourism.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUploadService _uploadService;

        public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IMapper mapper, IUploadService uploadService)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _uploadService = uploadService;
        }

        public async Task<UserProfileDto> GetCurrentUserAsync(string userId)
        {
            var user = await _userManager.Users
                .Include(u => u.Nationality)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User not found");

            var roles = await _userManager.GetRolesAsync(user);
            var dto = _mapper.Map<UserProfileDto>(user);
            dto.Roles = roles.ToList();

            return dto;
        }

        public async Task<UserProfileDto> GetUserByIdAsync(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.Nationality)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new NotFoundException("User not found");

            var roles = await _userManager.GetRolesAsync(user);
            var dto = _mapper.Map<UserProfileDto>(user);
            dto.Roles = roles.ToList();

            return dto;
        }

        public async Task<List<UserProfileDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users
                .Include(u => u.Nationality)
                .ToListAsync();

            var result = new List<UserProfileDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var dto = _mapper.Map<UserProfileDto>(user);
                dto.Roles = roles.ToList();
                result.Add(dto);
            }

            return result;
        }

        public async Task UpdateMyProfileAsync(string userId, UpdateUserDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found");

            user.FirstName = dto.FirstName ?? user.FirstName;
            user.LastName = dto.LastName ?? user.LastName;
            user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;

            if (dto.Image != null)
                user.ImageUrl = await _uploadService.UploadImageAsync(dto.Image, "avatars");

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
                throw new ValidationException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task DeleteMyProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                throw new ValidationException("Failed to delete account");
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
                throw new NotFoundException("User not found");

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                throw new ValidationException("Failed to delete user");
        }

        public async Task ChangePasswordAsync(string userId, ChangePasswordDto dto)
        {
            if (dto.NewPassword != dto.ConfirmNewPassword)
                throw new ValidationException("New passwords don't match");

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found");

            var result = await _userManager.ChangePasswordAsync(
                user, dto.CurrentPassword, dto.NewPassword);

            if (!result.Succeeded)
                throw new ValidationException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<int> GetUserPointsAsync(string userId)
        {
            var tourist = await _context.Tourists
                .FirstOrDefaultAsync(t => t.UserId == userId);

            if (tourist == null)
                throw new NotFoundException("Tourist profile not found");

            return tourist.Points;
        }
        // ─── Get Pending Guides ───
        public async Task<List<GuideDto>> GetPendingGuidesAsync()
        {
            var guides = await _context.Guides
                .Include(g => g.User)
                .Where(g => g.Status == GuideStatus.Pending)
                .ToListAsync();

            return guides.Select(MapGuideToDto).ToList();
        }

        // ─── Get All Guides ───
        public async Task<List<GuideDto>> GetAllGuidesAsync(bool activeOnly = false)
        {
            var query = _context.Guides.Include(g => g.User).AsQueryable();

            if (activeOnly)
                query = query.Where(g => g.Status == GuideStatus.Active);

            var guides = await query.ToListAsync();
            return guides.Select(MapGuideToDto).ToList();
        }
        // ─── Approve Guide ───
        public async Task ApproveGuideAsync(string guideId)
        {
            var guide = await _context.Guides
                .FirstOrDefaultAsync(g => g.UserId == guideId);

            if (guide == null)
                throw new NotFoundException("Guide not found");

            if (guide.Status == GuideStatus.Active)
                throw new ConflictException("Guide is already approved");

            if (guide.Status == GuideStatus.Rejected)
                throw new ValidationException("Cannot approve a rejected guide");

            guide.Status = GuideStatus.Active;

            await _context.SaveChangesAsync();
        }

        // ─── Reject Guide ───
        public async Task RejectGuideAsync(string guideId, string reason)
        {
            var guide = await _context.Guides
                .FirstOrDefaultAsync(g => g.UserId == guideId);

            if (guide == null)
                throw new NotFoundException("Guide not found");

            if (guide.Status == GuideStatus.Rejected)
                throw new ConflictException("Guide is already rejected");

            guide.Status = GuideStatus.Rejected;
            guide.EndDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        // ─── Get Guide Profile ────
        public async Task<GuideProfileDto> GetGuideProfileAsync(string guideId)
        {
            var guide = await _context.Guides
                .Include(g => g.User)
                .Include(g => g.GuideLanguages).ThenInclude(gl => gl.Language)
                .Include(g => g.GuideReviews)
                .FirstOrDefaultAsync(g => g.UserId == guideId);

            if (guide == null)
                throw new NotFoundException("Guide not found");

            return new GuideProfileDto
            {
                UserId = guide.UserId,
                FullName = $"{guide.User.FirstName} {guide.User.LastName}",
                Email = guide.User.Email!,
                ImageUrl = guide.User.ImageUrl,
                Status = guide.Status.ToString(),
                StartDate = guide.StartDate,
                Languages = guide.GuideLanguages.Select(gl => new GuideLanguageResponseDto
                {
                    LanguageId = gl.LanguageId,
                    LanguageName = gl.Language.Name,
                    Level = gl.Level.ToString()
                }).ToList(),
                AverageRating = guide.GuideReviews.Any()
                    ? guide.GuideReviews.Average(r => r.Rating)
                    : 0,
                ReviewsCount = guide.GuideReviews.Count
            };
        }

        // ─── Add Language ────
        public async Task AddGuideLanguageAsync(string guideId, GuideLanguageDto dto)
        {
            var guide = await _context.Guides.FirstOrDefaultAsync(g => g.UserId == guideId);

            if (guide == null)
                throw new NotFoundException("Guide not found");

            var exists = await _context.GuideLanguages
                .AnyAsync(gl => gl.GuideId == guideId && gl.LanguageId == dto.LanguageId);

            if (exists)
                throw new ConflictException("Language already added");

            _context.GuideLanguages.Add(new GuideLanguage
            {
                GuideId = guideId,
                LanguageId = dto.LanguageId,
                Level = dto.Level
            });

            await _context.SaveChangesAsync();
        }

        // ─── Remove Language ───
        public async Task RemoveGuideLanguageAsync(string guideId, int languageId)
        {
            var language = await _context.GuideLanguages
                .FirstOrDefaultAsync(gl => gl.GuideId == guideId && gl.LanguageId == languageId);

            if (language == null)
                throw new NotFoundException("Language not found");

            _context.GuideLanguages.Remove(language);
            await _context.SaveChangesAsync();
        }

        // ─── Suspend Guide ────
        public async Task SuspendGuideAsync(string guideId)
        {
            var guide = await _context.Guides.FirstOrDefaultAsync(g => g.UserId == guideId);

            if (guide == null)
                throw new NotFoundException("Guide not found");

            if (guide.Status == GuideStatus.Suspended)
                throw new ConflictException("Guide is already suspended");

            guide.Status = GuideStatus.Suspended;
            await _context.SaveChangesAsync();
        }

        // ─── Set Availability ───
        public async Task SetGuideAvailabilityAsync(string guideId, bool isOnline)
        {
            var guide = await _context.Guides.FirstOrDefaultAsync(g => g.UserId == guideId);

            if (guide == null)
                throw new NotFoundException("Guide not found");

            if (guide.Status == GuideStatus.Pending)
                throw new ValidationException("Guide is not approved yet");

            if (guide.Status == GuideStatus.Suspended)
                throw new ValidationException("Guide is suspended");

            guide.Status = isOnline ? GuideStatus.Active : GuideStatus.Offline;
            await _context.SaveChangesAsync();
        }

        // ─── Private Helper ───
        private static GuideDto MapGuideToDto(GuideProfile g) => new()
        {
            UserId = g.UserId,
            FullName = $"{g.User.FirstName} {g.User.LastName}",
            Email = g.User.Email!,
            LicenseNumber = g.LicenseNumber,
            LicenseImageUrl = g.LicenseImageUrl,
            Status = g.Status.ToString(),
            StartDate = g.StartDate
        };
    }
}