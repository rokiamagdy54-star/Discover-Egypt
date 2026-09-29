using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Payment.DTOs
{
    public class RefundDto
    {
        [Required]
        public int BookingId { get; set; }

        [Required]
        public string Reason { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Refund amount must be greater than 0")]
        public decimal? Amount { get; set; }
    }
}