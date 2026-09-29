namespace Tourism.Core.Entities
{
    public class PlacePhoto : BaseEntity
    {
        public int PlaceId { get; set; }
        public string ImageUrl { get; set; }
        public Place Place { get; set; }
    }
}