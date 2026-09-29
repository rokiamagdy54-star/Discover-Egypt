using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Review.DTOs
{
    public class CreatePlaceReviewDto
    {
        [Required]
        public int PlaceId { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [Required, MaxLength(1000)]
        public string Comment { get; set; }
    }
}