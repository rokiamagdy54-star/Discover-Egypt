using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Tourism.Core.Entities;
using Tourism.Core.Enum;
using Tourism.Core.Enums;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.Authentication.DTOs;
using Tourism.Core.Features.Authentication.Interfaces;
using Tourism.Core.Features.Email.Interfaces;
using Tourism.Core.Features.UploadImage.Interfaces;
using Tourism.Core.Helpers;
using Tourism.Repository.Data.DBContext;

namespace Tourism.Service.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly ISocialAuthService _socialAuthService;
        private readonly IUploadService _uploadService;
        private readonly IEmailService _emailService;

        public AuthService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, ITokenService tokenService, ISocialAuthService socialAuthService, IUploadService uploadService, IEmailService emailService)
        {
            _userManager = userManager;
            _context = context;
            _tokenService = tokenService;
            _socialAuthService = socialAuthService;
            _uploadService = uploadService;
            _emailService = emailService;
        }

        // Register 
        public async Task<AuthResponseDto> RegisterAsync(RegisterDto model)
        {
            if (model.Password != model.ConfirmPassword)
                throw new ValidationException("Passwords don't match");

            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                throw new ConflictException("Email already registered");

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                Gender = model.Gender,
                NationalityId = model.NationalityId
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ValidationException(errors);
            }

            await _userManager.AddToRoleAsync(user, model.Role.ToString());

            if (model.Role == UserRole.Tourist)
            {
                if (!string.IsNullOrEmpty(model.LicenseNumber) || model.LicenseImage != null)
                    throw new ValidationException("Tourist cannot provide license information");

                _context.Tourists.Add(new TouristProfile { UserId = user.Id });
            }
            else if (model.Role == UserRole.Guide)
            {
                if (string.IsNullOrEmpty(model.LicenseNumber))
                    throw new ValidationException("License number is required for guides");

                if (model.LicenseImage is null || model.LicenseImage.Length == 0)
                    throw new ValidationException("License image is required for guides");

                var imageUrl = await _uploadService.UploadImageAsync(model.LicenseImage, "licenses");

                _context.Guides.Add(new GuideProfile
                {
                    UserId = user.Id,
                    LicenseNumber = model.LicenseNumber,
                    LicenseImageUrl = imageUrl,
                    Status = GuideStatus.Pending,
                    StartDate = DateTime.UtcNow
                });

                if (model.Languages != null && model.Languages.Any())
                {
                    foreach (var lang in model.Languages)
                    {
                        _context.GuideLanguages.Add(new GuideLanguage
                        {
                            GuideId = user.Id,
                            LanguageId = lang.LanguageId,
                            Level = lang.Level
                        });
                    }
                }
            }

            await _context.SaveChangesAsync();

            var (token, expiresOn) = await _tokenService.GenerateTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponseDto
            {
                IsAuthenticated = true,
                Message = "Registered successfully",
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList(),
                Token = token,
                ExpiresOn = expiresOn
            };
        }

        // Login 
        public async Task<AuthResponseDto> LoginAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
                throw new ValidationException("Invalid email or password");

            var isValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!isValid)
                throw new ValidationException("Invalid email or password");

            var (token, expiresOn) = await _tokenService.GenerateTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponseDto
            {
                IsAuthenticated = true,
                Message = "Login successful",
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList(),
                Token = token,
                RefreshToken = refreshToken.Token,
                ExpiresOn = expiresOn
            };
        }

        // Social Login 
        public async Task<AuthResponseDto> SocialLogin(SocialLoginDto model)
        {
            var userInfo = await _socialAuthService.VerifyTokenAsync(model.Token, model.Provider);

            if (userInfo == null || string.IsNullOrEmpty(userInfo.Email))
                throw new ValidationException("Invalid social token");

            var user = await _userManager.FindByEmailAsync(userInfo.Email);

            if (user == null)
            {
                var names = userInfo.Name?.Split(' ') ?? [];
                var firstName = names.FirstOrDefault() ?? "User";
                var lastName = names.Skip(1).FirstOrDefault() ?? string.Empty;

                user = new ApplicationUser
                {
                    UserName = userInfo.Email,
                    Email = userInfo.Email,
                    FirstName = firstName,
                    LastName = lastName,
                    NationalityId = 1
                };

                var result = await _userManager.CreateAsync(user);

                if (!result.Succeeded)
                    throw new ValidationException("Failed to create user account");

                await _userManager.AddToRoleAsync(user, UserRole.Tourist.ToString());
                _context.Tourists.Add(new TouristProfile { UserId = user.Id });
                await _context.SaveChangesAsync();
            }

            var (jwt, expiresOn) = await _tokenService.GenerateTokenAsync(user);
            var refreshToken = GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponseDto
            {
                IsAuthenticated = true,
                Message = "Login successful",
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList(),
                Token = jwt,
                RefreshToken = refreshToken.Token,
                ExpiresOn = expiresOn
            };
        }

        // Refresh Token
        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

            if (user == null)
                throw new ValidationException("Invalid refresh token");

            var token = user.RefreshTokens.First(t => t.Token == refreshToken);

            if (!token.IsActive)
                throw new ValidationException("Refresh token is expired or revoked");

            // rotate token
            token.RevokedOn = DateTime.UtcNow;
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);

            var (jwt, expiresOn) = await _tokenService.GenerateTokenAsync(user);
            await _userManager.UpdateAsync(user);

            var roles = await _userManager.GetRolesAsync(user);

            return new AuthResponseDto
            {
                IsAuthenticated = true,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles.ToList(),
                Token = jwt,
                RefreshToken = newRefreshToken.Token,
                ExpiresOn = expiresOn
            };
        }

        // Revoke Token 
        public async Task RevokeTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

            if (user == null)
                throw new NotFoundException("Token not found");

            var token = user.RefreshTokens.First(t => t.Token == refreshToken);

            if (!token.IsActive)
                throw new ValidationException("Token is already revoked or expired");

            token.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        // Logout 
        public async Task LogoutAsync(string userId)
        {
            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                throw new NotFoundException("User not found");

            foreach (var token in user.RefreshTokens.Where(t => t.IsActive))
                token.RevokedOn = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
        }

        // OTP / Password Reset
        public async Task SendOtpAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return;

            // Rate limit 30 OTP
            var last = await _context.PasswordResetOtps
                .Where(x => x.UserId == user.Id)
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync();

            if (last != null && (DateTime.UtcNow - last.CreatedOn).TotalSeconds < 30)
                throw new ValidationException("Please wait 30 seconds before requesting another OTP");

            // Invalidate old OTPs
            var oldOtps = _context.PasswordResetOtps
                .Where(x => x.UserId == user.Id && !x.IsUsed);

            await oldOtps.ForEachAsync(o => o.IsUsed = true);

            var code = OtpHelper.GenerateOtp();

            _context.PasswordResetOtps.Add(new PasswordResetOtp
            {
                UserId = user.Id,
                CodeHash = OtpHelper.HashOtp(code),
                CreatedOn = DateTime.UtcNow,
                ExpiresOn = DateTime.UtcNow.AddMinutes(10),
                IsUsed = false
            });

            await _context.SaveChangesAsync();

            await _emailService.SendAsync(user.Email, "Your OTP Code – DiscoverEgypt", $"Your verification code is: {code}\n\nValid for 10 minutes.");
        }

        public async Task<bool> ResetWithOtpAsync(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null) return false;

            var otp = await _context.PasswordResetOtps
                .Where(x => x.UserId == user.Id && !x.IsUsed)
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync();

            if (otp == null || otp.ExpiresOn < DateTime.UtcNow)
                return false;

            if (!OtpHelper.VerifyOtp(dto.Otp, otp.CodeHash))
                return false;

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, resetToken, dto.NewPassword);

            if (!result.Succeeded) return false;

            otp.IsUsed = true;
            await _context.SaveChangesAsync();

            return true;
        }

        // Private Helper 
        private static RefreshToken GenerateRefreshToken() => new()
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            CreatedOn = DateTime.UtcNow,
            ExpiresOn = DateTime.UtcNow.AddDays(7)
        };
    }
}