using System.Text.Json;
using VikopApi.Application;
using VikopApi.Application.Comments;
using VikopApi.Application.Findings;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;
using VikopApi.Domain.Enums;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.FactoryTests
{
    [TestFixture]
    public class FindingFactoryTests
    {

        [Test]
        public void Create_ByAddFindingRequest()
        {
            var commentFactory = new CommentFactory();
            var factory = new FindingFactory(commentFactory);
            var request = new AddFindingRequest
            {
                CreatorId = "id",
                Description = "description",
                Link = "link",
                Picture = "pic",
                TagList = new List<string>(),
                Title = "title",
            };

            var finding = factory.Create(request);

            Assert.Multiple(() =>
            {
                Assert.That(finding.CreatorId, Is.EqualTo(request.CreatorId));
                Assert.That(finding.Picture, Is.EqualTo(request.Picture));
                Assert.That(finding.Created.GetDate(), Is.EqualTo(DateTime.Now.GetDate()));
                Assert.That(finding.Description, Is.EqualTo(request.Description));
                Assert.That(finding.Link, Is.EqualTo(request.Link));
                Assert.That(finding.Title, Is.EqualTo(request.Title));
            });
        }

        [Test]
        public void CreateListItem_ByFinding()
        {
            var commentFactory = new CommentFactory();
            var factory = new FindingFactory(commentFactory);
            var finding = new Finding
            {
                Comments = new List<FindingComment>(),
                Created = new DateTime(1, 1, 1),
                Creator = new ApplicationUser { UserName = "name", Rank = Rank.Green },
                CreatorId = "id",
                Description = "description",
                Id = 1,
                Link = "link",
                Picture = "pic",
                Reactions = new List<FindingReaction>(),
                Tags = new List<FindingTag> { new FindingTag { Tag = new Tag { Name = "tag" } } },
                Title = "title",
            };

            var item = factory.CreateListItem(finding);

            Assert.Multiple(() =>
            {
                Assert.That(item.CommentCount, Is.EqualTo(finding.Comments.Count()));
                Assert.That(item.Created, Is.EqualTo(finding.Created.GetTime()));
                Assert.That(item.CreatorName, Is.EqualTo(finding.Creator.UserName));
                Assert.That(item.CreatorId, Is.EqualTo(finding.CreatorId));
                Assert.That(item.CreatorRank, Is.EqualTo(finding.Creator.Rank));
                Assert.That(item.Description, Is.EqualTo(finding.Description));
                Assert.That(item.Id, Is.EqualTo(finding.Id));
                Assert.That(item.Link, Is.EqualTo(finding.Link));
                Assert.That(item.Reactions, Is.EqualTo(finding.Reactions.SumReactions()));
                Assert.That(item.TagList, Is.EquivalentTo(finding.Tags.Select(tag => tag.Tag)));
                Assert.That(item.Title, Is.EqualTo(finding.Title));
            });
        }

        [Test]
        public void CreateModel_ByFinding()
        {
            var commentFactory = new CommentFactory();
            var factory = new FindingFactory(commentFactory);
            var finding = new Finding
            {
                Comments = new List<FindingComment> { new FindingComment { Comment = new Comment 
                { 
                    Id = 1,
                    Content = "content",
                    Created = DateTime.Now,
                    Reactions = new List<CommentReaction>(),
                    Creator = new ApplicationUser() { Rank = Rank.Green, UserName = "name", Id = "id" }
                } } },
                Created = new DateTime(1, 1, 1),
                Creator = new ApplicationUser { UserName = "user", Rank = Rank.Red, Id = "id2" },
                CreatorId = "id",
                Description = "description",
                Id = 1,
                Link = "link",
                Picture = "pic",
                Reactions = new List<FindingReaction>(),
                Tags = new List<FindingTag> { new FindingTag { Tag = new Tag { Name = "tag" } } },
                Title = "title",
            };

            var model = factory.CreateModel(finding);
            var expectedCommentModels = finding.Comments.Select(comment => comment.Comment)
                .Select(comment => commentFactory.CreateModel(comment));

            Assert.Multiple(() =>
            {
                Assert.That(model.Finding.CommentCount, Is.EqualTo(finding.Comments.Count()));
                Assert.That(model.Finding.Created, Is.EqualTo(finding.Created.GetTime()));
                Assert.That(model.Finding.CreatorName, Is.EqualTo(finding.Creator.UserName));
                Assert.That(model.Finding.CreatorId, Is.EqualTo(finding.CreatorId));
                Assert.That(model.Finding.CreatorRank, Is.EqualTo(finding.Creator.Rank));
                Assert.That(model.Finding.Description, Is.EqualTo(finding.Description));
                Assert.That(model.Finding.Id, Is.EqualTo(finding.Id));
                Assert.That(model.Finding.Link, Is.EqualTo(finding.Link));
                Assert.That(model.Finding.Reactions, Is.EqualTo(finding.Reactions.SumReactions()));
                Assert.That(model.Finding.TagList, Is.EquivalentTo(finding.Tags.Select(tag => tag.Tag)));
                Assert.That(model.Finding.Title, Is.EqualTo(finding.Title));
                Assert.That(model.Comments.Select(x => JsonSerializer.Serialize(x)),
                    Is.EquivalentTo(expectedCommentModels.Select(x => JsonSerializer.Serialize(x))));
            });
        }
    }
}
