namespace Tourism.Core.Features.Favorite.DTOs
{
    public class FavoriteDto
    {
        public int PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string City { get; set; }
        public string CategoryName { get; set; }
        public DateTime AddedAt { get; set; }
    }
}