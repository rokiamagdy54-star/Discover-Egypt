using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Users.DTOs
{
    public class UpdateUserDto
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }
        public IFormFile? Image { get; set; } 

        [Phone]
        public string? PhoneNumber { get; set; }
    }
}