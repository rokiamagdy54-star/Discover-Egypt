namespace Tourism.Core.Features.Places.DTOs
{
    public class PlaceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? NameAr { get; set; }
        public string Description { get; set; }
        public string? DescriptionAr { get; set; }
        public string City { get; set; }
        public string? CityAr { get; set; }
        public decimal TicketPrice { get; set; }
        public string CategoryName { get; set; }
        public string? CategoryNameAr { get; set; }

        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public string? ImageUrl { get; set; }
        public List<string> Photos { get; set; } = new();
    }
}