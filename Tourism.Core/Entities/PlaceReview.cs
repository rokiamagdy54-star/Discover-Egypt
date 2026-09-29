using Tourism.Core.Entities;

namespace Tourism.Core.Entities
{
    public class PlaceReview : BaseEntity
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public string TouristId { get; set; }
        public TouristProfile Tourist { get; set; }
        public int PlaceId { get; set; }
        public Place Place { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}