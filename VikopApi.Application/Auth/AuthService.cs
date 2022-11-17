using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Models.User.Requests;

namespace VikopApi.Application.Auth
{
    [Implementation(typeof(IAuthService))]
    public class AuthService : IAuthService
    {
        private readonly string _authAudience;
        private readonly string _authIssuer;
        private readonly string _secretKey;
        private readonly string _placeholderImage;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(IConfiguration config,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager)
        {
            _authAudience = config["Auth:Audience"];
            _authIssuer = config["Auth:Issuer"];
            _secretKey = config["Auth:SecretKey"];
            _placeholderImage = config["Image:Placeholder"];
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<string> AddUser(AddUserRequest request)
        {
            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                ProfilePicture = _placeholderImage,
                Rank = 0,
                Created = DateTime.Now
            };

            await _userManager.CreateAsync(user, request.Password);

            return user.Id;
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
