using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Places.DTOs
{
    public class CreatePlaceDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; }
        public string? NameAr { get; set; }

        [Required]
        public string Description { get; set; }
        public string? DescriptionAr { get; set; }

        [Required, MaxLength(100)]
        public string City { get; set; }
        public string? CityAr { get; set; }

        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

        public TimeSpan AverageVisitDuration { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TicketPrice { get; set; }

        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public IFormFile? MainImage { get; set; }
        public List<IFormFile>? Photos { get; set; }
    }
}