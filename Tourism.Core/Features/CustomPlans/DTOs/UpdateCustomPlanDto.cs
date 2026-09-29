using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.CustomPlans.DTOs
{
    public class UpdateCustomPlanDto
    {
        [MaxLength(200)]
        public string? Title { get; set; }
        public string? TitleAr { get; set; }
        public string? Description { get; set; }
        public string? DescriptionAr { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string? Notes { get; set; }
        public string? Destination { get; set; }
        public IFormFile? Image { get; set; }
    }
}