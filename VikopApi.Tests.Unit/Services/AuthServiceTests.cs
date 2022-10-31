using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Moq;
using System.Text;
using VikopApi.Application.Auth;
using VikopApi.Application.Models.Requests;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Services
{
    [TestFixture]
    public class AuthServiceTests
    {
        private Mock<IConfiguration> GetConfigMock()
        {
            var config = new Mock<IConfiguration>();
            config.SetupGet(x => x[It.Is<string>(s => s == "Auth:Audience")]).Returns("https://localhost");
            config.SetupGet(x => x[It.Is<string>(s => s == "Auth:Issuer")]).Returns("https://localhost");
            config.SetupGet(x => x[It.Is<string>(s => s == "Auth:SecretKey")]).Returns("supersecretkeyloooooooooooooooooooooooooooooooong");
            config.SetupGet(x => x[It.Is<string>(s => s == "Image:Placeholder")]).Returns("default.jpg");
            return config;
        }

        private Mock<UserManager<ApplicationUser>> GetUserManagerMock()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            return userManager;
        }

        [Test]
        public async Task AddUser()
        {
            var dbContext = Extensions.GetAppDbContext();
            var config = GetConfigMock();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var userManager = GetUserManagerMock();
            userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .Callback((ApplicationUser user, string password) =>
                {
                    user.Id = Guid.NewGuid().ToString();
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }).ReturnsAsync(IdentityResult.Success);

            var service = new AuthService(config.Object, httpContextAccessor.Object, userManager.Object);
            var request = new AddUserRequest
            {
                Email = "email@email.com",
                Password = "zaq1@WSX",
                Username = "username"
            };

            var res = await service.AddUser(request);

            var newUser = dbContext.Users.FirstOrDefault(x => x.Id == res);

            Assert.Multiple(() =>
            {
                Assert.That(newUser, Is.Not.Null);
                Assert.That(newUser.Email, Is.EqualTo(request.Email));
                Assert.That(newUser.UserName, Is.EqualTo(request.Username));
                Assert.That(newUser.ProfilePicture, Is.EqualTo(config.Object["Image:Placeholder"]));
            });
        }

        [Test]
        public void GetCurrentUserId()
        {
            var dbContext = Extensions.GetAppDbContext();
            var config = GetConfigMock();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            var userPrincipal = new ClaimsPrincipal();
            userPrincipal.AddIdentity(new ClaimsIdentity(new List<Claim> { new Claim(JwtRegisteredClaimNames.Sub, "id") }));

            httpContextAccessor.Setup(x => x.HttpContext.User).Returns(userPrincipal);
            var userManager = GetUserManagerMock();
            userManager
                .Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userPrincipal.Identities.First().Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value);

            var service = new AuthService(config.Object, httpContextAccessor.Object, userManager.Object);

            var res = service.GetCurrentUserId();

            Assert.That(res, Is.EqualTo("id"));
        }

        [Test]
        public async Task GetToken()
        {
            var dbContext = Extensions.GetAppDbContext();
            var config = GetConfigMock();
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            var userManager = GetUserManagerMock();
            userManager.Setup(x => x.GetClaimsAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(new List<Claim>());
            var service = new AuthService(config.Object, httpContextAccessor.Object, userManager.Object);

            var user = new ApplicationUser { Email = "email@email.com", Id = "id", UserName = "name" };

            var token = await service.GetToken(user);

            var tokenValidation = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Object["Auth:SecretKey"])),
                ValidIssuer = config.Object["Auth:Issuer"],
                ValidAudience = config.Object["Auth:Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var res = tokenHandler.ValidateToken(tokenHandler.WriteToken(token), tokenValidation, out SecurityToken validToken);
            
            Assert.Multiple(() =>
            {
                Assert.That(res.Claims.Any(x => x.Value == user.Id));
                Assert.That(res.Claims.Any(x => x.Value == user.Email));
                Assert.That(res.Claims.Any(x => x.Value == user.UserName));
            });
        }
    }
}
