using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Places.DTOs
{
    public class UpdatePlaceDto
    {
        [MaxLength(200)]
        public string? Name { get; set; }
        public string? NameAr { get; set; }

        public string? Description { get; set; }
        public string? DescriptionAr { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }
        public string? CityAr { get; set; }

        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public TimeSpan? AverageVisitDuration { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? TicketPrice { get; set; }

        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? ClosingTime { get; set; }
        public int? CategoryId { get; set; }
        public IFormFile? MainImage { get; set; }
    }
}