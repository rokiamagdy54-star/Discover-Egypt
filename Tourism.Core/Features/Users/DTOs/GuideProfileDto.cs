namespace Tourism.Core.Features.Users.DTOs
{
    public class GuideProfileDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? ImageUrl { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public List<GuideLanguageResponseDto> Languages { get; set; } = new();
        public double AverageRating { get; set; }
        public int ReviewsCount { get; set; }
    }

    public class GuideLanguageResponseDto
    {
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        public string Level { get; set; }
    }
}