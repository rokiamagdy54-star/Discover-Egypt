using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Community.DTOs
{
    public class CreatePostDto
    {
        [Required, MaxLength(5000)]
        public string Content { get; set; }

        [MaxLength(200)]
        public string? Title { get; set; }

        public List<IFormFile>? Images { get; set; }
    }
}