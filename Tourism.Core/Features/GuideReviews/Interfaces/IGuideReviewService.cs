using Tourism.Core.Features.GuideReviews.DTOs;

namespace Tourism.Core.Features.GuideReviews.Interfaces
{
    public interface IGuideReviewService
    {
        Task AddAsync(string userId, CreateGuideReviewDto dto);
        Task<List<GuideReviewDto>> GetByGuideAsync(string guideId);
        Task DeleteAsync(int reviewId, string userId);
    }
}
