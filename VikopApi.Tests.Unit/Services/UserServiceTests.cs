using Moq;
using VikopApi.Application.Findings.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Posts.Abstractions;
using VikopApi.Application.User;
using VikopApi.Application.User.Abstractions;
using VikopApi.Domain.Enums;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        [Test]
        [TestCase("id1")]
        [TestCase("id2")]
        [TestCase("id3")]
        [TestCase("id4")]
        [TestCase("id5")]
        public void GetUserById(string userId)
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1", UserName = "name1" },
                new ApplicationUser { Id = "id2", UserName = "name2" },
                new ApplicationUser { Id = "id3", UserName = "name3" },
                new ApplicationUser { Id = "id4", UserName = "name4" },
            };

            var userManagerMock = new Mock<IApplicationUserManager>();
            userManagerMock.Setup(x => x.GetUserById(It.IsAny<string>(), It.IsAny<Func<ApplicationUser, UserModel>>()))
                .Returns((string id, Func<ApplicationUser, UserModel> selector)
                    => users.Where(x => x.Id == id).Select(selector).FirstOrDefault());

            var userFactoryMock = new Mock<IUserFactory>();
            userFactoryMock.Setup(x => x.CreateModel(It.IsAny<ApplicationUser>()))
                .Returns((ApplicationUser user) => new UserModel { Id = user.Id, UserName = user.UserName });

            var findingFactoryMock = new Mock<IFindingFactory>();
            var postFactoryMock = new Mock<IPostFactory>();

            var service = new UserService(userManagerMock.Object, userFactoryMock.Object, findingFactoryMock.Object, postFactoryMock.Object);

            var res = service.GetUserById(userId);

            var user = users.FirstOrDefault(x => x.Id == userId);

            Assert.That(res?.UserName, Is.EqualTo(user?.UserName));
        }

        [Test]
        [TestCase("id1")]
        [TestCase("id2")]
        [TestCase("id3")]
        public void GetUserFindings(string userId)
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser 
                { 
                    Id = "id1",
                    UserName = "name1",
                    Findings = new List<Finding>
                    {
                        new Finding { Id = 1 },
                        new Finding { Id = 2 },
                        new Finding { Id = 3 },
                    }
                },
                new ApplicationUser 
                { 
                    Id = "id2",
                    UserName = "name2",
                    Findings = new List<Finding>
                    {
                        new Finding { Id = 4 },
                    }
                },
                new ApplicationUser 
                { 
                    Id = "id3", 
                    UserName = "name3",
                    Findings = new List<Finding>
                    {
                        new Finding { Id = 5 },
                        new Finding { Id = 6 },
                    }
                },
            };

            var userManagerMock = new Mock<IApplicationUserManager>();
            userManagerMock.Setup(x => x.GetUserFindings(It.IsAny<string>(), It.IsAny<Func<Finding, FindingListItemModel>>()))
                .Returns((string id, Func<Finding, FindingListItemModel> selector)
                    => users.FirstOrDefault(x => x.Id == id).Findings.Select(selector));

            var userFactoryMock = new Mock<IUserFactory>();

            var findingFactoryMock = new Mock<IFindingFactory>();
            findingFactoryMock.Setup(x => x.CreateListItem(It.IsAny<Finding>()))
                .Returns((Finding finding) => new FindingListItemModel { Id = finding.Id });

            var postFactoryMock = new Mock<IPostFactory>();

            var service = new UserService(userManagerMock.Object, userFactoryMock.Object, findingFactoryMock.Object, postFactoryMock.Object);

            var res = service.GetUserFindings(userId);

            var user = users.FirstOrDefault(x => x.Id == userId);

            Assert.Multiple(() =>
            {
                Assert.That(res.Count(), Is.EqualTo(user.Findings.Count));
                Assert.That(res.All(x => user.Findings.Any(y => y.Id == x.Id)));
            });
        }

        [Test]
        [TestCase("id1")]
        [TestCase("id2")]
        [TestCase("id3")]
        public void GetUserPosts(string userId)
        {
            var posts = new List<Post>
            {
                new Post { Comment = new Comment { Id = 1, CreatorId = "id1" } },
                new Post { Comment = new Comment { Id = 2, CreatorId = "id2" } },
                new Post { Comment = new Comment { Id = 3, CreatorId = "id3" } },
                new Post { Comment = new Comment { Id = 4, CreatorId = "id1" } },
                new Post { Comment = new Comment { Id = 5, CreatorId = "id2" } },
                new Post { Comment = new Comment { Id = 6, CreatorId = "id3" } },
                new Post { Comment = new Comment { Id = 7, CreatorId = "id3" } },
            };

            var userManagerMock = new Mock<IApplicationUserManager>();
            userManagerMock.Setup(x => x.GetUserPosts(It.IsAny<string>(), It.IsAny<Func<Post, PostModel>>()))
                .Returns((string id, Func<Post, PostModel> selector)
                    => posts.Where(x => x.Comment.CreatorId == id).Select(selector));

            var userFactoryMock = new Mock<IUserFactory>();

            var findingFactoryMock = new Mock<IFindingFactory>();

            var postFactoryMock = new Mock<IPostFactory>();
            postFactoryMock.Setup(x => x.CreateModel(It.IsAny<Post>()))
                .Returns((Post post) => new PostModel { Content = new CommentModel { Id = post.Comment.Id, CreatorId = post.Comment.CreatorId } });

            var service = new UserService(userManagerMock.Object, userFactoryMock.Object, findingFactoryMock.Object, postFactoryMock.Object);

            var res = service.GetUserPosts(userId);

            var expectedPosts = posts.Where(x => x.Comment.CreatorId == userId);

            Assert.Multiple(() =>
            {
                Assert.That(res.Count(), Is.EqualTo(expectedPosts.Count()));
                Assert.That(res.All(x => expectedPosts.Any(y => y.Comment.Id == x.Content.Id)));
            });
        }

        [Test]
        public void GetUsers()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1", UserName = "name1" },
                new ApplicationUser { Id = "id2", UserName = "name2" },
                new ApplicationUser { Id = "id3", UserName = "name3" },
                new ApplicationUser { Id = "id4", UserName = "name4" },
            };

            var userManagerMock = new Mock<IApplicationUserManager>();
            userManagerMock.Setup(x => x.GetUsers(It.IsAny<Func<ApplicationUser, UserListItemModel>>()))
                .Returns((Func<ApplicationUser, UserListItemModel> selector) => users.Select(selector));

            var userFactoryMock = new Mock<IUserFactory>();
            userFactoryMock.Setup(x => x.CreateListItem(It.IsAny<ApplicationUser>()))
                .Returns((ApplicationUser user) => new UserListItemModel { Id = user.Id, Username = user.UserName });

            var findingFactoryMock = new Mock<IFindingFactory>();
            var postFactoryMock = new Mock<IPostFactory>();

            var service = new UserService(userManagerMock.Object, userFactoryMock.Object, findingFactoryMock.Object, postFactoryMock.Object);

            var res = service.GetUsers();

            Assert.Multiple(() =>
            {
                Assert.That(res.Count(), Is.EqualTo(users.Count));
                Assert.That(res.All(x => users.Any(y => y.Id == x.Id)));
            });
        }

        [Test]
        [TestCase("email1@email.com", true)]
        [TestCase("email2@email.com", true)]
        [TestCase("email3@email.com", true)]
        [TestCase("email4@email.com", true)]
        [TestCase("EMAIL1@email.com", true)]
        [TestCase("EMAIL2@email.com", true)]
        [TestCase("EMAIL3@email.com", true)]
        [TestCase("EMAIL4@email.com", true)]
        [TestCase("email5@email.com", false)]
        public void IsEmailOccupied(string userEmail, bool expectedResult)
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1", Email = "email1@email.com" },
                new ApplicationUser { Id = "id2", Email = "email2@email.com" },
                new ApplicationUser { Id = "id3", Email = "email3@email.com" },
                new ApplicationUser { Id = "id4", Email = "email4@email.com" },
            };

            var userManagerMock = new Mock<IApplicationUserManager>();
            userManagerMock.Setup(x => x.GetUsers(It.IsAny<Func<ApplicationUser, string>>()))
                .Returns((Func<ApplicationUser, string> selector) => users.Select(selector));

            var userFactoryMock = new Mock<IUserFactory>();

            var findingFactoryMock = new Mock<IFindingFactory>();
            var postFactoryMock = new Mock<IPostFactory>();

            var service = new UserService(userManagerMock.Object, userFactoryMock.Object, findingFactoryMock.Object, postFactoryMock.Object);

            var res = service.IsEmailOccupied(userEmail);

            Assert.That(res, Is.EqualTo(expectedResult));
        }

        [Test]
        public async Task UpdateRanks()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1", Rank = Rank.Green },
                new ApplicationUser { Id = "id2", Rank = Rank.Green },
                new ApplicationUser { Id = "id3", Rank = Rank.Green },
                new ApplicationUser { Id = "id4", Rank = Rank.Green },
            };

            var userManagerMock = new Mock<IApplicationUserManager>();
            userManagerMock.Setup(x => x.UpdateRanks())
                .Callback(() => users.ForEach(x => x.Rank = Rank.Orange))
                .ReturnsAsync(true);

            var userFactoryMock = new Mock<IUserFactory>();

            var findingFactoryMock = new Mock<IFindingFactory>();
            var postFactoryMock = new Mock<IPostFactory>();

            var service = new UserService(userManagerMock.Object, userFactoryMock.Object, findingFactoryMock.Object, postFactoryMock.Object);

            var res = await service.UpdateRanks();

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(users.All(x => x.Rank == Rank.Orange));
            });
        }

        [Test]
        public async Task UpdateUser()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { Id = "id1", UserName = "name1" },
                new ApplicationUser { Id = "id2", UserName = "name2" },
                new ApplicationUser { Id = "id3", UserName = "name3" },
                new ApplicationUser { Id = "id4", UserName = "name4" },
            };

            var userManagerMock = new Mock<IApplicationUserManager>();
            userManagerMock.Setup(x => x.UpdateUser(It.IsAny<string>(), It.IsAny<Action<ApplicationUser>>()))
                .Callback((string id, Action<ApplicationUser> action) =>
                {
                    var user = users.FirstOrDefault(x => x.Id == id);
                    action(user);
                });

            var userFactoryMock = new Mock<IUserFactory>();

            var findingFactoryMock = new Mock<IFindingFactory>();
            var postFactoryMock = new Mock<IPostFactory>();

            var service = new UserService(userManagerMock.Object, userFactoryMock.Object, findingFactoryMock.Object, postFactoryMock.Object);

            var request = new UpdateUserRequest
            {
                Id = "id3",
                Picture = "newPic",
                UserName = "username"
            };

            await service.UpdateUser(request);

            var user = users.FirstOrDefault(x => x.Id == request.Id);

            Assert.Multiple(() =>
            {
                Assert.That(user.UserName, Is.EqualTo(request.UserName));
                Assert.That(user.ProfilePicture, Is.EqualTo(request.Picture));
            });
        }
    }
}
