namespace Tourism.Core.Features.CustomPlans.DTOs
{
    public class CustomPlanResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? TitleAr { get; set; }
        public string Description { get; set; }
        public string? DescriptionAr { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string TouristId { get; set; }
        public string? Notes { get; set; }
        public string? Destination { get; set; }
        public string Status { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}