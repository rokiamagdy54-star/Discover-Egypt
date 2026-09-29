using AutoMapper;
using Tourism.Core.Entities;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.GuideReviews.DTOs;
using Tourism.Core.Features.GuideReviews.Interfaces;
using Tourism.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Tourism.Core.Enum;

namespace Tourism.Service
{
    public class GuideReviewService : IGuideReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GuideReviewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddAsync(string userId, CreateGuideReviewDto dto)
        {
            // Check if the booking exists and belongs to the user, and is associated with the guide
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(dto.BookingId);

            if (booking == null)
                throw new NotFoundException("Booking not found");

            if (booking.TouristId != userId)
                throw new ForbiddenException("You don't have access to this booking");

            if (booking.GuideId != dto.GuideId)
                throw new ValidationException("Guide is not associated with this booking");

            if (booking.Status != BookingStatus.Confirmed)
                throw new ValidationException("You can only review after a confirmed booking");

            var exists = await _unitOfWork.Repository<GuideReview>().GetFirstAsync(
                predicate: r => r.TouristId == userId && r.BookingId == dto.BookingId);

            if (exists != null)
                throw new ConflictException("You have already reviewed this booking");

            var review = new GuideReview
            {
                TouristId = userId,
                GuideId = dto.GuideId,
                BookingId = dto.BookingId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            await _unitOfWork.Repository<GuideReview>().AddAsync(review);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<List<GuideReviewDto>> GetByGuideAsync(string guideId)
        {
            var reviews = await _unitOfWork.Repository<GuideReview>().GetAllAsync(
                predicate: r => r.GuideId == guideId,
                include: q => q.Include(r => r.Tourist).ThenInclude(t => t.User));

            return _mapper.Map<List<GuideReviewDto>>(
                reviews.OrderByDescending(r => r.CreatedAt));
        }

        public async Task DeleteAsync(int reviewId, string userId)
        {
            var review = await _unitOfWork.Repository<GuideReview>().GetByIdAsync(reviewId);

            if (review == null)
                throw new NotFoundException("Review not found");

            if (review.TouristId != userId)
                throw new ForbiddenException("You don't have access to this review");

            _unitOfWork.Repository<GuideReview>().Delete(review);
            await _unitOfWork.CompleteAsync();
        }
    }
}