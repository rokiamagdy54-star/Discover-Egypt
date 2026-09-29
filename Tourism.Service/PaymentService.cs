using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tourism.Core.Entities;
using Tourism.Core.Enum;
using Tourism.Core.Enums;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.Payment.DTOs;
using Tourism.Core.Features.Payment.Interfaces;
using Tourism.Core.Interfaces;

namespace Tourism.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PaymentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Pay
        public async Task<PaymentDto> PayAsync(string userId, PayDto dto)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(dto.BookingId);

            if (booking == null)
                throw new NotFoundException("Booking not found");

            if (booking.TouristId != userId)
                throw new ForbiddenException("You don't have access to this booking");

            if (booking.PaymentMethod == PaymentMethod.Cash)
                throw new ValidationException("Cash bookings do not require online payment");

            if (booking.PaymentStatus == PaymentStatus.Paid)
                throw new ConflictException("Booking is already paid");

            if (booking.Status == BookingStatus.Cancelled)
                throw new ValidationException("Cannot pay a cancelled booking");

            var payment = new Payment
            {
                BookingId = booking.Id,
                Amount = booking.Amount,
                PaymentMethod = booking.PaymentMethod,
                Status = PaymentStatus.Paid,
                CreatedAt = DateTime.UtcNow
            };

            booking.PaymentStatus = PaymentStatus.Paid;
            booking.Status = BookingStatus.Confirmed;

            await _unitOfWork.Repository<Payment>().AddAsync(payment);
            _unitOfWork.Repository<Booking>().Update(booking);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<PaymentDto>(payment);
        }

        // Refund
        public async Task<PaymentDto> RefundAsync(string userId, RefundDto dto)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(dto.BookingId);

            if (booking == null)
                throw new NotFoundException("Booking not found");

            if (booking.TouristId != userId)
                throw new ForbiddenException("You don't have access to this booking");

            if (booking.PaymentStatus != PaymentStatus.Paid)
                throw new ValidationException("Cannot refund an unpaid booking");

            if (booking.Status == BookingStatus.Cancelled)
                throw new ConflictException("Booking is already cancelled");

            var refundAmount = dto.Amount ?? booking.Amount;

            if (refundAmount > booking.Amount)
                throw new ValidationException("Refund amount cannot exceed the original payment amount");

            var refund = new Payment
            {
                BookingId = booking.Id,
                Amount = refundAmount,
                PaymentMethod = booking.PaymentMethod,
                Status = PaymentStatus.Refunded,
                CreatedAt = DateTime.UtcNow
            };

            booking.PaymentStatus = PaymentStatus.Refunded;
            booking.Status = BookingStatus.Cancelled;
            booking.CancelledAt = DateTime.UtcNow;
            booking.CancelReason = dto.Reason;

            await _unitOfWork.Repository<Payment>().AddAsync(refund);
            _unitOfWork.Repository<Booking>().Update(booking);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<PaymentDto>(refund);
        }

        // Get Booking Payments
        public async Task<List<PaymentDto>> GetBookingPaymentsAsync(string userId, int bookingId)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(bookingId);

            if (booking == null)
                throw new NotFoundException("Booking not found");

            if (booking.TouristId != userId)
                throw new ForbiddenException("You don't have access to this booking");

            var payments = await _unitOfWork.Repository<Payment>().GetAllAsync(
                predicate: p => p.BookingId == bookingId);

            return _mapper.Map<List<PaymentDto>>(payments);
        }
    }
}