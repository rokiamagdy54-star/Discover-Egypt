using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.ReadyPlans.DTOs
{
    public class CreateReadyPlanDto
    {
        [Required, MaxLength(200)]
        public string Title { get; set; }
        public string? TitleAr { get; set; }

        [Required]
        public string Description { get; set; }
        public string? DescriptionAr { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [Required]
        public string GuideId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public IFormFile? Image { get; set; }
        public List<int> PlaceIds { get; set; } = new();
    }
}