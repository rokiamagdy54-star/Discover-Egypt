using Tourism.Core.Features.Authentication.DTOs;
using Tourism.Core.Features.Users.DTOs;

namespace Tourism.Core.Features.Users.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> GetCurrentUserAsync(string userId);
        Task<UserProfileDto> GetUserByIdAsync(string id);
        Task<List<UserProfileDto>> GetAllUsersAsync();
        Task UpdateMyProfileAsync(string userId, UpdateUserDto dto);
        Task DeleteMyProfileAsync(string userId);
        Task DeleteUserAsync(string id);
        Task ChangePasswordAsync(string userId, ChangePasswordDto dto);
        Task<int> GetUserPointsAsync(string userId);
        Task<List<GuideDto>> GetPendingGuidesAsync();
        Task<List<GuideDto>> GetAllGuidesAsync(bool activeOnly = false);
        Task ApproveGuideAsync(string guideId);
        Task RejectGuideAsync(string guideId, string reason);
        Task AddGuideLanguageAsync(string guideId, GuideLanguageDto dto);
        Task RemoveGuideLanguageAsync(string guideId, int languageId);
        Task SuspendGuideAsync(string guideId);
        Task SetGuideAvailabilityAsync(string guideId, bool isOnline);
        Task<GuideProfileDto> GetGuideProfileAsync(string guideId);
    }
}