using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Users.DTOs
{
    public class ChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required, MinLength(8)]
        public string NewPassword { get; set; }

        [Required]
        public string ConfirmNewPassword { get; set; }
    }
}