using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.GuideReviews.DTOs
{
    public class CreateGuideReviewDto
    {
        [Required]
        public string GuideId { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [Required, MaxLength(1000)]
        public string Comment { get; set; }
    }
}
