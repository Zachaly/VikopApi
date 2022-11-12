using System.IdentityModel.Tokens.Jwt;
using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.Auth.Abstractions
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> GetToken(ApplicationUser user);
        string GetCurrentUserId();
        Task<string> AddUser(AddUserRequest request);
    }
}
