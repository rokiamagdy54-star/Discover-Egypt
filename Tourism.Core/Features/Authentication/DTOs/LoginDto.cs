using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Authentication.DTOs
{
    public class LoginDto
    {
        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}