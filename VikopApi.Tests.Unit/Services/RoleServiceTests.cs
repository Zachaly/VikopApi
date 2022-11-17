using Microsoft.AspNetCore.Identity;
using Moq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using VikopApi.Application.Models;
using VikopApi.Application.Models.User;
using VikopApi.Application.Role;
using VikopApi.Application.User;
using VikopApi.Application.User.Abstractions;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Services
{
    [TestFixture]
    public class RoleServiceTests
    {
        private Mock<UserManager<ApplicationUser>> GetUserManagerMock()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            return userManager;
        }

        [Test]
        public async Task AddRole()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1" },
                new ApplicationUser { Id = "id2" },
                new ApplicationUser { Id = "id3" },
            };

            var claims = new List<IdentityUserClaim<string>>();


            var userManagerMock = GetUserManagerMock();
            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => users.FirstOrDefault(x => x.Id == id));
            userManagerMock.Setup(x => x.AddClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .Callback((ApplicationUser user, Claim claim) =>
                {
                    claims.Add(new IdentityUserClaim<string> { UserId = user.Id, ClaimType = claim.Type, ClaimValue = claim.Value });
                }).ReturnsAsync(new IdentityResult());

            var factoryMock = new Mock<IUserFactory>();

            var service = new RoleService(userManagerMock.Object, factoryMock.Object);

            var res = await service.AddRole("id1", "Moderator");

            Assert.That(claims.Any(x => x.ClaimType == "Role" && x.ClaimValue == "Moderator" && x.UserId == "id1"));
        }

        [Test]
        public async Task RemoveRole()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1" },
                new ApplicationUser { Id = "id2" },
                new ApplicationUser { Id = "id3" },
            };

            var claims = new List<IdentityUserClaim<string>>
            {
                new IdentityUserClaim<string> { UserId = "id1", ClaimType = "Role", ClaimValue = "Moderator" },
                new IdentityUserClaim<string> { UserId = "id2", ClaimType = "Role", ClaimValue = "Admin" }
            };


            var userManagerMock = GetUserManagerMock();
            userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => users.FirstOrDefault(x => x.Id == id));
            userManagerMock.Setup(x => x.RemoveClaimAsync(It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .Callback((ApplicationUser user, Claim claim) =>
                {
                    claims.Remove(claims.FirstOrDefault(x => x.UserId == user.Id && x.ClaimValue == claim.Value));
                }).ReturnsAsync(new IdentityResult());

            var factoryMock = new Mock<IUserFactory>();

            var service = new RoleService(userManagerMock.Object, factoryMock.Object);

            var res = await service.RemoveRole("id1", "Moderator");

            Assert.That(!claims.Any(x => x.UserId == "id1" && x.ClaimValue == "Moderator"));
        }

        [Test]
        public async Task GetUsersWithRole()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1" },
                new ApplicationUser { Id = "id2" },
                new ApplicationUser { Id = "id3" },
            };

            var claims = new List<IdentityUserClaim<string>>
            {
                new IdentityUserClaim<string> { UserId = "id1", ClaimType = "Role", ClaimValue = "Moderator" },
                new IdentityUserClaim<string> { UserId = "id2", ClaimType = "Role", ClaimValue = "Admin" },
                new IdentityUserClaim<string> { UserId = "id3", ClaimType = "Role", ClaimValue = "Moderator" }
            };


            var userManagerMock = GetUserManagerMock();
            userManagerMock.Setup(x => x.GetUsersForClaimAsync(It.IsAny<Claim>()))
                .ReturnsAsync((Claim claim)
                    => users.Where(x => claims.Any(y => y.UserId == x.Id && y.ClaimValue == claim.Value && y.ClaimType == claim.Type)).ToList());


            var factoryMock = new Mock<IUserFactory>();
            factoryMock.Setup(x => x.CreateListItem(It.IsAny<ApplicationUser>()))
                .Returns((ApplicationUser user) => new UserListItemModel { Id = user.Id });

            var service = new RoleService(userManagerMock.Object, factoryMock.Object);

            var res = await service.GetUsersWithRole("Moderator");

            Assert.Multiple(() =>
            {
                Assert.That(res.Count(), Is.EqualTo(claims.Where(x => x.ClaimValue == "Moderator").Count()));
                Assert.That(res.All(x => claims.Any(y => y.UserId == x.Id && y.ClaimValue == "Moderator")));
            });
        }
    }
}
