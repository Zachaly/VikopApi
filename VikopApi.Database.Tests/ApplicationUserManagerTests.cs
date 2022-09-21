using VikopApi.Domain.Infractructure;

namespace VikopApi.Database.Tests
{
    public class ApplicationUserManagerTests : DatabaseTest
    {
        private readonly IApplicationUserManager _appUserManager;

        public ApplicationUserManagerTests() : base()
        {
            _appUserManager = new ApplicationUserManager(_dbContext);
        }

        [Fact]
        public void Get_Users()
        {
            var users = _appUserManager.GetUsers(x => x);

            Assert.Equal(3, users.Count());
        }

        [Fact]
        public void Get_User_By_Id()
        {
            var user = _appUserManager.GetUserById("1", x => x);

            Assert.Equal("1", user.Id);
            Assert.Equal("user1", user.UserName);
            Assert.Single(user.Findings);
            Assert.Equal(4, user.Comments.Count());
        }

        [Fact]
        public void Get_User_By_Nonexistent_Id()
        {
            var user = _appUserManager.GetUserById("2137", x => x);

            Assert.Null(user);
        }
    }
}
