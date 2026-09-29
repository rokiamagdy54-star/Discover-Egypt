using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tourism.Core.Features.Payment.DTOs;
using Tourism.Core.Features.Payment.Interfaces;

namespace Tourism.API.Controllers
{
    [Route("api/payments")]
    [ApiController]
    [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        /// <summary>Processes a Visa payment for a pending booking.</summary>
        [HttpPost("pay")]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> Pay([FromBody] PayDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _paymentService.PayAsync(userId, dto);
            return Ok(result);
        }

        /// <summary>Processes a refund for a paid booking.</summary>
        [HttpPost("refund")]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> Refund([FromBody] RefundDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var result = await _paymentService.RefundAsync(userId, dto);
            return Ok(result);
        }

        /// <summary>Gets all payments for a specific booking.</summary>
        [HttpGet("booking/{bookingId}")]
        public async Task<IActionResult> GetBookingPayments(int bookingId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var payments = await _paymentService.GetBookingPaymentsAsync(userId, bookingId);
            return Ok(payments);
        }
    }
}