using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Community.DTOs
{
    public class UpdateCommentDto
    {
        [Required, MaxLength(2000)]
        public string Content { get; set; }

        public List<IFormFile>? NewImages { get; set; }
        public List<int>? DeleteImageIds { get; set; }
    }
}