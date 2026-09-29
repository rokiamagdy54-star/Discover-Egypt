namespace Tourism.Core.Entities
{
    public class GuideReview : BaseEntity
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string TouristId { get; set; }
        public TouristProfile Tourist { get; set; }
        public string GuideId { get; set; }
        public GuideProfile Guide { get; set; }
        public int BookingId { get; set; } //
        public Booking Booking { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
