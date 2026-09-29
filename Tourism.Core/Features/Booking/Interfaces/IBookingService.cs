using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism.Core.Features.Booking.DTOs;

namespace Tourism.Core.Features.Booking.Interfaces
{
    public interface IBookingService
    {
        Task<BookingDto> CreateBookingAsync(string userId, CreateBookingDto dto);
        Task<List<BookingDto>> GetUserBookingsAsync(string userId);
        Task<BookingDto> GetBookingByIdAsync(int id, string userId);
        Task CancelBookingAsync(int id, string userId, string reason);
        Task<List<BookingDto>> GetGuideBookingsAsync(string guideId);
        Task ConfirmBookingAsync(int id, string guideId);
        Task<List<BookingDto>> GetAllBookingsAsync();
    }
}
