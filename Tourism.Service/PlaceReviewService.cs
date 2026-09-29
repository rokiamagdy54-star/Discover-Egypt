using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tourism.Core.Entities;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.Review.DTOs;
using Tourism.Core.Features.Review.Interfaces;
using Tourism.Core.Interfaces;

namespace Tourism.Service
{
    public class PlaceReviewService : IPlaceReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PlaceReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Add Review
        public async Task AddReviewAsync(string userId, CreatePlaceReviewDto dto)
        {
            var place = await _unitOfWork.Repository<Place>().GetByIdAsync(dto.PlaceId);
            if (place == null)
                throw new NotFoundException("Place not found");

            var exists = await _unitOfWork.Repository<PlaceReview>().GetFirstAsync(
                predicate: r => r.TouristId == userId && r.PlaceId == dto.PlaceId);

            if (exists != null)
                throw new ConflictException("You have already reviewed this place");

            var review = new PlaceReview
            {
                TouristId = userId,
                PlaceId = dto.PlaceId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<PlaceReview>().AddAsync(review);
            await _unitOfWork.CompleteAsync();
        }

        // Get Reviews By Place
        public async Task<List<PlaceReviewDto>> GetReviewsByPlaceAsync(int placeId)
        {
            var reviews = await _unitOfWork.Repository<PlaceReview>().GetAllAsync(
                predicate: r => r.PlaceId == placeId,
                include: q => q.Include(r => r.Tourist)
                               .ThenInclude(t => t.User));

            return _mapper.Map<List<PlaceReviewDto>>(
                reviews.OrderByDescending(r => r.CreatedAt));
        }

        // Update Review
        public async Task UpdateReviewAsync(int reviewId, string userId, UpdatePlaceReviewDto dto)
        {
            var review = await _unitOfWork.Repository<PlaceReview>().GetByIdAsync(reviewId);

            if (review == null)
                throw new NotFoundException("Review not found");

            if (review.TouristId != userId)
                throw new ForbiddenException("You don't have access to this review");

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;

            _unitOfWork.Repository<PlaceReview>().Update(review);
            await _unitOfWork.CompleteAsync();
        }

        // Delete Review
        public async Task DeleteReviewAsync(int reviewId, string userId)
        {
            var review = await _unitOfWork.Repository<PlaceReview>().GetByIdAsync(reviewId);

            if (review == null)
                throw new NotFoundException("Review not found");

            if (review.TouristId != userId)
                throw new ForbiddenException("You don't have access to this review");

            _unitOfWork.Repository<PlaceReview>().Delete(review);
            await _unitOfWork.CompleteAsync();
        }
    }
}