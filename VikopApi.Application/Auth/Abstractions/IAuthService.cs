using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
