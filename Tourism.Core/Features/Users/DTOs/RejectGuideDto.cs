using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Users.DTOs
{
    public class RejectGuideDto
    {
        [Required]
        public string Reason { get; set; }
    }
}