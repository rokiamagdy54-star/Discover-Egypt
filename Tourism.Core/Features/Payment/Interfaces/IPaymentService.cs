using Tourism.Core.Features.Payment.DTOs;

namespace Tourism.Core.Features.Payment.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentDto> PayAsync(string userId, PayDto dto);
        Task<PaymentDto> RefundAsync(string userId, RefundDto dto);
        Task<List<PaymentDto>> GetBookingPaymentsAsync(string userId, int bookingId);
    }
}