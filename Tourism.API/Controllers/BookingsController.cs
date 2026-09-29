using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism.Core.Features.Booking.DTOs;
using Tourism.Core.Features.Booking.Interfaces;

namespace Tourism.API.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    [Authorize]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        /// <summary>Creates a new booking reservation.</summary>
        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> CreateBooking([FromBody] CreateBookingDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _bookingService.CreateBookingAsync(userId, dto);
            return StatusCode(201, result);
        }

        /// <summary>Retrieves all bookings for the current tourist.</summary>
        [HttpGet("my")]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var bookings = await _bookingService.GetUserBookingsAsync(userId);
            return Ok(bookings);
        }

        /// <summary>Retrieves a specific booking by ID.</summary>
        [HttpGet("{id}")]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var booking = await _bookingService.GetBookingByIdAsync(id, userId);
            return Ok(booking);
        }

        /// <summary>Cancels a specific booking.</summary>
        [HttpPut("{id}/cancel")]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> CancelBooking(int id, [FromBody] CancelBookingDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _bookingService.CancelBookingAsync(id, userId, dto.Reason);
            return Ok(new { message = "Booking cancelled successfully" });
        }

        /// <summary>Retrieves all bookings assigned to the current guide.</summary>
        [HttpGet("guide")]
        [Authorize(Roles = "Guide")]
        public async Task<IActionResult> GetGuideBookings()
        {
            var guideId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var bookings = await _bookingService.GetGuideBookingsAsync(guideId);
            return Ok(bookings);
        }

        /// <summary>Guide confirms a specific booking.</summary>
        [HttpPut("{id}/confirm")]
        [Authorize(Roles = "Guide")]
        public async Task<IActionResult> ConfirmBooking(int id)
        {
            var guideId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            await _bookingService.ConfirmBookingAsync(id, guideId);
            return Ok(new { message = "Booking confirmed successfully" });
        }

        /// <summary>Retrieves all bookings in the system. Admin only.</summary>
        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _bookingService.GetAllBookingsAsync();
            return Ok(bookings);
        }
    }
}