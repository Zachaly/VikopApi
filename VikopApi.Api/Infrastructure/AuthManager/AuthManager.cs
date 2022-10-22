using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VikopApi.Domain.Models;

namespace VikopApi.Api.Infrastructure.AuthManager
{
    [Implementation(typeof(IAuthManager))]
    public class AuthManager : IAuthManager
    {
        private readonly string _authAudience;
        private readonly string _authIssuer;
        private readonly string _secretKey;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthManager(IConfiguration config,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _authAudience = config["Auth:Audience"];
            _authIssuer = config["Auth:Issuer"];
            _secretKey = config["Auth:SecretKey"];
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string GetCurrentUserId()
            => _userManager.GetUserId(_httpContextAccessor.HttpContext.User);

        public async Task<JwtSecurityToken> GetToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            };

            claims.AddRange(await _userManager.GetClaimsAsync(user));

            var bytes = Encoding.UTF8.GetBytes(_secretKey);
            var key = new SymmetricSecurityKey(bytes);

            var algorithm = SecurityAlgorithms.HmacSha256;

            var credentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                _authAudience,
                _authIssuer,
                claims,
                DateTime.Now,
                DateTime.Now.AddHours(24),
                credentials);

            return token;
        }
    }
}
