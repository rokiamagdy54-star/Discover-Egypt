using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tourism.Core.Features.Roles.DTOs;
using Tourism.Core.Features.Roles.Interfaces;

namespace Tourism.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>Retrieves all roles in the system.</summary>
        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetRolesAsync();
            return Ok(roles);
        }

        /// <summary>Creates a new role.</summary>
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto dto)
        {
            await _roleService.CreateRoleAsync(dto);
            return StatusCode(201, new { message = "Role created successfully" });
        }

        /// <summary>Deletes a role by ID.</summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            await _roleService.DeleteRoleAsync(id);
            return Ok(new { message = "Role deleted successfully" });
        }
    }
}