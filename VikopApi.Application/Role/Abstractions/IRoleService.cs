using Microsoft.AspNetCore.Identity;
using VikopApi.Application.Models;

namespace VikopApi.Application.Role.Abstractions
{
    public interface IRoleService
    {
        Task<IdentityResult> AddRole(string userId, string role);
        Task<IEnumerable<UserListItemModel>> GetUsersWithRole(string role);
        Task<IdentityResult> RemoveRole(string userId, string role);
    }
}
