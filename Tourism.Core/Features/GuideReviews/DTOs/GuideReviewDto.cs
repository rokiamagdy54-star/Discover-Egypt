namespace Tourism.Core.Features.GuideReviews.DTOs
{
    public class GuideReviewDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string TouristName { get; set; }
        public int BookingId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
