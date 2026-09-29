using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Payment.DTOs
{
    public class PayDto
    {
        [Required]
        public int BookingId { get; set; }
    }
}