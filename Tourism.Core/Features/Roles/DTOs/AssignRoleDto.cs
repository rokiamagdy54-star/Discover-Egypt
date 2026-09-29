using System.ComponentModel.DataAnnotations;

namespace Tourism.Core.Features.Roles.DTOs
{
    public class AssignRoleDto
    {
        [Required]
        public string RoleName { get; set; }
    }
}