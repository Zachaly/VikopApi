using Microsoft.EntityFrameworkCore;
using VikopApi.Database;
using VikopApi.Domain.Enums;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Managers
{
    [TestFixture]
    public class ApplicationUserManagerTests
    {
        [Test]
        [TestCase("id1")]
        [TestCase("id2")]
        [TestCase("id3")]
        [TestCase("id4")]
        public void GetUser_ById(string searchId)
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1" },
                new ApplicationUser { Id = "id2" },
                new ApplicationUser { Id = "id3" },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(userList);
            var manager = new ApplicationUserManager(dbContext);

            var user = manager.GetUserById(searchId, x => x);

            Assert.That(user, Is.EqualTo(userList.FirstOrDefault(x => x.Id == searchId)));
        }

        [Test]
        [TestCase("id1")]
        [TestCase("id2")]
        [TestCase("id3")]
        [TestCase("id4")]
        public void GetUserFindings_ById(string userId)
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1" },
                new ApplicationUser { Id = "id2" },
                new ApplicationUser { Id = "id3" },
            };
            var findingList = new List<Finding>
            {
                new Finding { CreatorId = "id1" },
                new Finding { CreatorId = "id2" },
                new Finding { CreatorId = "id3" },
                new Finding { CreatorId = "id1" },
                new Finding { CreatorId = "id2" },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(userList);
            dbContext.AddContent(findingList);

            var manager = new ApplicationUserManager(dbContext);

            var userFindings = manager.GetUserFindings(userId, x => x);

            Assert.That(userFindings, Is.EquivalentTo(findingList.Where(x => x.CreatorId == userId)));
        }

        [Test]
        [TestCase("id1")]
        [TestCase("id2")]
        [TestCase("id3")]
        [TestCase("id4")]
        public void GetUserPosts_ById(string userId)
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1" },
                new ApplicationUser { Id = "id2" },
                new ApplicationUser { Id = "id3" },
            };
            var postList = new List<Post>
            {
                new Post
                {
                    Comment = new Comment { CreatorId = "id1" }
                },
                new Post
                {
                    Comment = new Comment { CreatorId = "id2" }
                },
                new Post
                {
                    Comment = new Comment { CreatorId = "id3" }
                },
                new Post
                {
                    Comment = new Comment { CreatorId = "id2" }
                },
                new Post
                {
                    Comment = new Comment { CreatorId = "id3" }
                },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(postList);
            dbContext.AddContent(userList);

            var manager = new ApplicationUserManager(dbContext);

            var userPosts = manager.GetUserPosts(userId, x => x);

            Assert.That(userPosts, Is.EquivalentTo(postList.Where(x => x.Comment.CreatorId == userId)));
        }

        [Test]
        public void GetUsers()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1" },
                new ApplicationUser { Id = "id2" },
                new ApplicationUser { Id = "id3" },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(userList);
            var manager = new ApplicationUserManager(dbContext);

            var user = manager.GetUsers(x => x);

            Assert.That(user, Is.EquivalentTo(userList));
        }

        [Test]
        public async Task UpdateRanks_RanksUpdated()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1", Created = DateTime.Now, Rank = Rank.Green },
                new ApplicationUser { Id = "id2", Created = DateTime.Now.AddMonths(-1).AddDays(-1) , Rank = Rank.Green },
                new ApplicationUser { Id = "id3", Created = DateTime.Now.AddYears(-1).AddDays(-1) , Rank = Rank.Orange },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(userList);
            var manager = new ApplicationUserManager(dbContext);

            var result = await manager.UpdateRanks();

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(userList[0].Rank, Is.EqualTo(Rank.Green));
                Assert.That(userList[1].Rank, Is.EqualTo(Rank.Orange));
                Assert.That(userList[2].Rank, Is.EqualTo(Rank.Red));
            });
        }

        [Test]
        public async Task UpdateRanks_RanksNotUpdated()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1", Created = DateTime.Now, Rank = Rank.Green },
                new ApplicationUser { Id = "id2", Created = DateTime.Now, Rank = Rank.Green },
                new ApplicationUser { Id = "id3", Created = DateTime.Now, Rank = Rank.Orange },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(userList);
            var manager = new ApplicationUserManager(dbContext);

            var result = await manager.UpdateRanks();

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.True);
                Assert.That(userList[0].Rank, Is.EqualTo(Rank.Green));
                Assert.That(userList[1].Rank, Is.EqualTo(Rank.Green));
                Assert.That(userList[2].Rank, Is.EqualTo(Rank.Orange));
            });
        }

        [Test]
        public async Task UpdateUser_Success()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1", UserName = "name1" },
                new ApplicationUser { Id = "id2", UserName = "name2" },
                new ApplicationUser { Id = "id3", UserName = "name3" },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(userList);
            var manager = new ApplicationUserManager(dbContext);
            var userId = "id1";
            var newName = "newname";

            var result = await manager.UpdateUser(userId, user => user.UserName = newName);

            Assert.Multiple(() =>
            {
                Assert.That(userList.First(user => user.Id == userId).UserName, Is.EqualTo(newName));
                Assert.That(!userList.Where(user => user.Id != userId).Any(user => user.UserName == newName));
            });
        }

        [Test]
        public void UpdateUser_InvalidId_ThrowsException()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1", UserName = "name1" },
                new ApplicationUser { Id = "id2", UserName = "name2" },
                new ApplicationUser { Id = "id3", UserName = "name3" },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(userList);
            var manager = new ApplicationUserManager(dbContext);

            Assert.ThrowsAsync<DbUpdateException>(async () => await manager.UpdateUser("id4", user => user.UserName = "newname"));
        }
    }
}
