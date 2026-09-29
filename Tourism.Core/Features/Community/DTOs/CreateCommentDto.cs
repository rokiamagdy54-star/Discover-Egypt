using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Community.DTOs
{
    public class CreateCommentDto
    {
        [Required]
        public int PostId { get; set; }

        public int? ParentCommentId { get; set; } 

        [Required, MaxLength(2000)]
        public string Content { get; set; }

        public List<IFormFile>? Images { get; set; }
    }
}