using Tourism.Core.Features.Review.DTOs;

namespace Tourism.Core.Features.Review.Interfaces
{
    public interface IPlaceReviewService
    {
        Task AddReviewAsync(string userId, CreatePlaceReviewDto dto);
        Task<List<PlaceReviewDto>> GetReviewsByPlaceAsync(int placeId);
        Task UpdateReviewAsync(int reviewId, string userId, UpdatePlaceReviewDto dto);
        Task DeleteReviewAsync(int reviewId, string userId);
    }
}