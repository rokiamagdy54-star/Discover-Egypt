using Tourism.Core.Features.Roles.DTOs;

namespace Tourism.Core.Features.Roles.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<string>> GetRolesAsync();
        Task CreateRoleAsync(CreateRoleDto dto);
        Task DeleteRoleAsync(string roleId);
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task AssignRoleToUserAsync(string userId, string roleName);
        Task RemoveRoleFromUserAsync(string userId, string roleName);
    }
}