namespace Tourism.Core.Features.Users.DTOs
{
    public class GuideDto
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string LicenseNumber { get; set; }
        public string LicenseImageUrl { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
    }
}