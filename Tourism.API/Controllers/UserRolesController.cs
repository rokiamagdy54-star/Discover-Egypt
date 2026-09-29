using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tourism.Core.Features.Roles.DTOs;
using Tourism.Core.Features.Roles.Interfaces;

namespace Tourism.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserRolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public UserRolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>Retrieves all roles assigned to a specific user.</summary>
        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            var roles = await _roleService.GetUserRolesAsync(id);
            return Ok(roles);
        }

        /// <summary>Assigns a role to a user.</summary>
        [HttpPost("{id}/assign-role")]
        public async Task<IActionResult> AssignRole(string id, [FromBody] AssignRoleDto dto)
        {
            await _roleService.AssignRoleToUserAsync(id, dto.RoleName);
            return Ok(new { message = "Role assigned successfully" });
        }

        /// <summary>Removes a role from a user.</summary>
        [HttpDelete("{id}/remove-role")]
        public async Task<IActionResult> RemoveRole(string id, [FromBody] AssignRoleDto dto)
        {
            await _roleService.RemoveRoleFromUserAsync(id, dto.RoleName);
            return Ok(new { message = "Role removed successfully" });
        }
    }
}