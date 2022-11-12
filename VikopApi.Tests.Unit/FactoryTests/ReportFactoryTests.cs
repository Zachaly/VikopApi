using VikopApi.Application;
using VikopApi.Application.Comments;
using VikopApi.Application.Findings;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Posts;
using VikopApi.Application.Reports;
using VikopApi.Application.User;
using VikopApi.Domain.Enums;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.FactoryTests
{
    [TestFixture]
    public class ReportFactoryTests
    {
        private ReportFactory CreateFactory()
        {
            var commentFactory = new CommentFactory();
            var findingFactory = new FindingFactory(commentFactory);
            var postFactory = new PostFactory(commentFactory);
            var userFactory = new UserFactory();

            return new ReportFactory(findingFactory, postFactory, userFactory);
        }

        [Test]
        public void Create_Finding()
        {
            var factory = CreateFactory();
            var request = new AddReportRequest
            {
                ObjectId = 1,
                Reason = "reason",
                ReportingUserId = "id"
            };

            var report = factory.CreateFinding(request);

            Assert.Multiple(() =>
            {
                Assert.That(report.FindingId, Is.EqualTo(request.ObjectId));
                Assert.That(report.Reason, Is.EqualTo(request.Reason));
                Assert.That(report.ReportingUserId, Is.EqualTo(request.ReportingUserId));
            });
        }

        [Test]
        public void Create_Post()
        {
            var factory = CreateFactory();
            var request = new AddReportRequest
            {
                ObjectId = 1,
                Reason = "reason",
                ReportingUserId = "id"
            };

            var report = factory.CreatePost(request);

            Assert.Multiple(() =>
            {
                Assert.That(report.PostId, Is.EqualTo(request.ObjectId));
                Assert.That(report.Reason, Is.EqualTo(request.Reason));
                Assert.That(report.ReportingUserId, Is.EqualTo(request.ReportingUserId));
            });
        }

        [Test]
        public void CreateModel_Post()
        {
            var factory = CreateFactory();
            var report = new PostReport
            {
                Created = new DateTime(1, 1, 1),
                Id = 1,
                Post = new Post
                {
                    Comment = new Comment
                    {
                        Id = 2,
                        Content = "content",
                        Created = new DateTime(2, 2, 2),
                        Creator = new ApplicationUser { UserName = "name", Rank = Rank.Green },
                        CreatorId = "id",
                        Picture = "",
                        Reactions = new List<CommentReaction>(),
                        SubComments = new List<SubComment>()
                    },
                    CommentId = 2,
                    Id = 3,
                    Tags = new List<PostTag>()
                },
                PostId = 3,
                Reason = "reason",
                ReportingUser = new ApplicationUser { Id = "id1", UserName = "username", Rank = Rank.Orange },
                ReportingUserId = "id1"
            };

            var model = factory.CreateModel(report);

            Assert.Multiple(() =>
            {
                Assert.That(model.Id, Is.EqualTo(report.Id));
                Assert.That(model.Created, Is.EqualTo(report.Created.GetTime()));
                Assert.That(model.ReportingUser, Is.Not.Null);
                Assert.That(model.Post, Is.Not.Null);
                Assert.That(model.Reason, Is.EqualTo(report.Reason));
            });
        }

        [Test]
        public void CreateModel_Finding()
        {
            var factory = CreateFactory();
            var report = new FindingReport
            {
                Created = new DateTime(1, 1, 1),
                Id = 1,
                Finding = new Finding
                {
                    Id = 2,
                    Comments = new List<FindingComment>(),
                    Created = new DateTime(2, 2, 2),
                    Creator = new ApplicationUser { Id = "id", UserName = "name", Rank = Rank.Green },
                    CreatorId = "id",
                    Description = "description",
                    Link = "link",
                    Picture = "",
                    Reactions = new List<FindingReaction>(),
                    Tags = new List<FindingTag>(),
                    Title = "title"
                },
                FindingId = 2,
                Reason = "reason",
                ReportingUser = new ApplicationUser { Id = "id1", UserName = "username", Rank = Rank.Orange },
                ReportingUserId = "id1"
            };

            var model = factory.CreateModel(report);

            Assert.Multiple(() =>
            {
                Assert.That(model.Id, Is.EqualTo(report.Id));
                Assert.That(model.Created, Is.EqualTo(report.Created.GetTime()));
                Assert.That(model.ReportingUser, Is.Not.Null);
                Assert.That(model.Finding, Is.Not.Null);
                Assert.That(model.Reason, Is.EqualTo(report.Reason));
            });
        }

        [Test]
        public void CreateListItem_Post()
        {
            var factory = CreateFactory();
            var report = new PostReport
            {
                Id = 1,
                Created = DateTime.Now,
                PostId = 2,
                ReportingUser = new ApplicationUser { UserName = "username", Rank = Rank.Orange, Id = "id" },
            };

            var item = factory.CreateListItem(report);

            Assert.Multiple(() =>
            {
                Assert.That(item.ReportingUser, Is.Not.Null);
                Assert.That(item.Id, Is.EqualTo(report.Id));
                Assert.That(item.ObjectId, Is.EqualTo(report.PostId));
                Assert.That(item.Created, Is.EqualTo(report.Created.GetTime()));
            });
        }

        [Test]
        public void CreateListItem_Finding()
        {
            var factory = CreateFactory();
            var report = new FindingReport
            {
                Id = 1,
                Created = DateTime.Now,
                FindingId = 2,
                ReportingUser = new ApplicationUser { UserName = "username", Rank = Rank.Orange, Id = "id" },
            };

            var item = factory.CreateListItem(report);

            Assert.Multiple(() =>
            {
                Assert.That(item.ReportingUser, Is.Not.Null);
                Assert.That(item.Id, Is.EqualTo(report.Id));
                Assert.That(item.ObjectId, Is.EqualTo(report.FindingId));
                Assert.That(item.Created, Is.EqualTo(report.Created.GetTime()));
            });
        }
    }
}
