using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using VikopApi.Application.Models;
using VikopApi.Application.Role.Abstractions;
using VikopApi.Application.User.Abstractions;

namespace VikopApi.Application.Role
{
    [Implementation(typeof(IRoleService))]
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserFactory _userFactory;

        public RoleService(UserManager<ApplicationUser> userManager, IUserFactory userFactory)
        {
            _userManager = userManager;
            _userFactory = userFactory;
        }

        public async Task<IdentityResult> AddRole(string userId, string role)
        {
            var claim = new Claim("Role", role);

            var user = await _userManager.FindByIdAsync(userId);

            return await _userManager.AddClaimAsync(user, claim);
        }

        public async Task<IdentityResult> RemoveRole(string userId, string role)
        {
            var claim = new Claim("Role", role);

            var user = await _userManager.FindByIdAsync(userId);

            return await _userManager.RemoveClaimAsync(user, claim);
        }

        public async Task<IEnumerable<UserListItemModel>> GetUsersWithRole(string role)
            => (await _userManager.GetUsersForClaimAsync(new Claim("Role", role)))
                .Select(user => _userFactory.CreateListItem(user));
    }
}
