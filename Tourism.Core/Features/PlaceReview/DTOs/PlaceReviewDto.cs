namespace Tourism.Core.Features.Review.DTOs
{
    public class PlaceReviewDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string TouristName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}