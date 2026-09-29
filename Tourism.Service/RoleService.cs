using Microsoft.AspNetCore.Identity;
using Tourism.Core.Entities;
using Tourism.Core.Exceptions;
using Tourism.Core.Features.Roles.DTOs;
using Tourism.Core.Features.Roles.Interfaces;

namespace Tourism.Service
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<IEnumerable<string>> GetRolesAsync()
            => _roleManager.Roles.Select(r => r.Name).ToList();

        public async Task CreateRoleAsync(CreateRoleDto dto)
        {
            if (await _roleManager.RoleExistsAsync(dto.Name))
                throw new ConflictException("Role already exists");

            var result = await _roleManager.CreateAsync(new IdentityRole(dto.Name));

            if (!result.Succeeded)
                throw new ValidationException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
                throw new NotFoundException("Role not found");

            var result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
                throw new ValidationException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found");

            return await _userManager.GetRolesAsync(user);
        }

        public async Task AssignRoleToUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found");

            if (!await _roleManager.RoleExistsAsync(roleName))
                throw new NotFoundException("Role not found");

            var alreadyInRole = await _userManager.IsInRoleAsync(user, roleName);

            if (alreadyInRole)
                throw new ConflictException("User already has this role");

            var result = await _userManager.AddToRoleAsync(user, roleName);

            if (!result.Succeeded)
                throw new ValidationException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        public async Task RemoveRoleFromUserAsync(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found");

            var isInRole = await _userManager.IsInRoleAsync(user, roleName);

            if (!isInRole)
                throw new ValidationException("User does not have this role");

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);

            if (!result.Succeeded)
                throw new ValidationException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
}