using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.RequestGuide.DTOs
{
    public class CreateRequestDto
    {
        [Required]
        public int TripId { get; set; }

        [Required]
        public string GuideId { get; set; }
    }
}