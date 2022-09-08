using System.IdentityModel.Tokens.Jwt;
using VikopApi.Domain.Models;

namespace VikopApi.Api.Infrastructure.AuthManager
{
    public interface IAuthManager
    {
        Task<JwtSecurityToken> GetToken(ApplicationUser user);
        string GetCurrentUserId();
    }
}
