namespace Tourism.Core.Features.ReadyPlans.DTOs
{
    public class ReadyPlanResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? TitleAr { get; set; }
        public string Description { get; set; }
        public string? DescriptionAr { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string GuideId { get; set; }
        public string? GuideName { get; set; }
        public int CompanyId { get; set; }
        public string? ImageUrl { get; set; }
        public List<int> PlaceIds { get; set; }
        public string Status { get; set; }
    }
}