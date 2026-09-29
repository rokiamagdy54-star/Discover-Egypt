using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tourism.Core.Entities;
using Tourism.Core.Enum;
using Tourism.Core.Enums;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.Booking.DTOs;
using Tourism.Core.Features.Booking.Interfaces;
using Tourism.Core.Interfaces;
using Tourism.Repository.Data.DBContext;

namespace Tourism.Service
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, ApplicationDbContext context, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
        }

        public async Task<BookingDto> CreateBookingAsync(string userId, CreateBookingDto dto)
        {
            if (dto.EndDate <= dto.StartDate)
                throw new ValidationException("End date must be after start date");

            if (dto.StartDate < DateTime.UtcNow)
                throw new ValidationException("Start date cannot be in the past");

            if (dto.NumberOfPeople <= 0)
                throw new ValidationException("Number of people must be at least 1");

            var plan = await _unitOfWork.Repository<BasePlan>().GetByIdAsync(dto.PlanId);
            if (plan == null)
                throw new NotFoundException("Plan not found");

            var tourist = await _context.Tourists
                .FirstOrDefaultAsync(t => t.UserId == userId);
            if (tourist == null)
                throw new NotFoundException("Tourist profile not found");

            GuideProfile guide = null;
            if (!string.IsNullOrEmpty(dto.GuideId))
            {
                guide = await _context.Guides
                    .Include(g => g.User)
                    .FirstOrDefaultAsync(g => g.UserId == dto.GuideId);

                if (guide == null)
                    throw new NotFoundException("Guide not found");
            }

            var totalAmount = plan.Price * dto.NumberOfPeople;
            if (guide != null) totalAmount += 1000;

            if (dto.UsePoints)
            {
                if (plan.RequiredPoints == null)
                    throw new ValidationException("This plan cannot be booked with points");

                if (tourist.Points < plan.RequiredPoints)
                    throw new ValidationException("Not enough points");

                totalAmount = 0;
                tourist.Points -= plan.RequiredPoints.Value;
            }

            var (paymentStatus, bookingStatus) = GetInitialStatus(dto.PaymentMethod);

            var booking = new Booking
            {
                TouristId = userId,
                PlanId = plan.Id,
                GuideId = guide?.UserId,
                BookingStart = dto.StartDate,
                BookingEnd = dto.EndDate,
                NumberOfPeople = dto.NumberOfPeople,
                Amount = totalAmount,
                PaymentMethod = dto.PaymentMethod,
                PaymentStatus = paymentStatus,
                Status = bookingStatus
            };

            await _unitOfWork.Repository<Booking>().AddAsync(booking);

            if (dto.PaymentMethod == PaymentMethod.Cash)
            {
                var payment = new Payment
                {
                    Booking = booking,
                    Amount = booking.Amount,
                    PaymentMethod = PaymentMethod.Cash,
                    Status = PaymentStatus.Paid,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.Repository<Payment>().AddAsync(payment);
            }

            if (bookingStatus == BookingStatus.Confirmed)
                tourist.Points += 50;

            await _unitOfWork.CompleteAsync();

            return _mapper.Map<BookingDto>(booking);
        }

        public async Task<List<BookingDto>> GetUserBookingsAsync(string userId)
        {
            var bookings = await _unitOfWork.Repository<Booking>().GetAllAsync(
                predicate: b => b.TouristId == userId,
                include: q => q.Include(b => b.Plan)
                               .Include(b => b.Guide).ThenInclude(g => g.User));

            return _mapper.Map<List<BookingDto>>(bookings);
        }

        public async Task<BookingDto> GetBookingByIdAsync(int id, string userId)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetFirstAsync(
                predicate: b => b.Id == id && b.TouristId == userId,
                include: q => q.Include(b => b.Plan)
                               .Include(b => b.Guide).ThenInclude(g => g.User));

            if (booking == null)
                throw new NotFoundException("Booking not found");

            return _mapper.Map<BookingDto>(booking);
        }

        public async Task CancelBookingAsync(int id, string userId, string reason)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(id);

            if (booking == null)
                throw new NotFoundException("Booking not found");

            if (booking.TouristId != userId)
                throw new ForbiddenException("You don't have access to this booking");

            if (booking.Status == BookingStatus.Cancelled)
                throw new ValidationException("Booking is already cancelled");

            if (booking.Amount == 0)
            {
                var tourist = await _context.Tourists
                    .FirstOrDefaultAsync(t => t.UserId == userId);
                var plan = await _unitOfWork.Repository<BasePlan>()
                    .GetByIdAsync(booking.PlanId);

                if (plan?.RequiredPoints != null && tourist != null)
                    tourist.Points += plan.RequiredPoints.Value;
            }

            booking.Status = BookingStatus.Cancelled;
            booking.CancelledAt = DateTime.UtcNow;
            booking.CancelReason = reason;

            _unitOfWork.Repository<Booking>().Update(booking);
            await _unitOfWork.CompleteAsync();
        }

        private static (PaymentStatus, BookingStatus) GetInitialStatus(PaymentMethod method) =>
            method switch
            {
                PaymentMethod.Cash => (PaymentStatus.Paid, BookingStatus.Confirmed),
                PaymentMethod.Visa => (PaymentStatus.Pending, BookingStatus.Pending),
                _ => throw new ValidationException("Invalid payment method")
            };

        // Get Guide Bookings
        public async Task<List<BookingDto>> GetGuideBookingsAsync(string guideId)
        {
            var bookings = await _unitOfWork.Repository<Booking>().GetAllAsync(
                predicate: b => b.GuideId == guideId,
                include: q => q.Include(b => b.Plan)
                               .Include(b => b.Guide).ThenInclude(g => g.User));

            return _mapper.Map<List<BookingDto>>(bookings);
        }

        // Confirm Booking (Guide)
        public async Task ConfirmBookingAsync(int id, string guideId)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(id);

            if (booking == null)
                throw new NotFoundException("Booking not found");

            if (booking.GuideId != guideId)
                throw new ForbiddenException("You don't have access to this booking");

            if (booking.Status == BookingStatus.Cancelled)
                throw new ValidationException("Cannot confirm a cancelled booking");

            if (booking.Status == BookingStatus.Confirmed)
                throw new ConflictException("Booking is already confirmed");

            booking.Status = BookingStatus.Confirmed;

            _unitOfWork.Repository<Booking>().Update(booking);
            await _unitOfWork.CompleteAsync();
        }

        // Get All Bookings (Admin)
        public async Task<List<BookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _unitOfWork.Repository<Booking>().GetAllAsync(
                include: q => q.Include(b => b.Plan)
                               .Include(b => b.Guide).ThenInclude(g => g.User));

            return _mapper.Map<List<BookingDto>>(bookings);
        }
    }
}