using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tourism.Core.Enum;
using Tourism.Core.Enums;

namespace Tourism.Core.Features.Authentication.DTOs
{
    public class RegisterDto
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Gender Gender { get; set; }

        [Required, MinLength(8), MaxLength(256)]
        public string Password { get; set; }

        [Required, MaxLength(256)]
        public string ConfirmPassword { get; set; }

        [Required]
        public int NationalityId { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public List<GuideLanguageDto>? Languages { get; set; }
        public string? LicenseNumber { get; set; }
        public IFormFile? LicenseImage { get; set; }
    }
}