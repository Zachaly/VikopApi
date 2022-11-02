using VikopApi.Application;
using VikopApi.Application.Comments;
using VikopApi.Application.Models.Requests;
using VikopApi.Domain.Enums;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.FactoryTests
{
    [TestFixture]
    public class CommentFactoryTests
    {
        [Test]
        public void Create_ByAddCommentRequest()
        {
            var factory = new CommentFactory();
            var request = new AddCommentRequest
            {
                Content = "content",
                CreatorId = "id",
                Picture = "pic"
            };

            var comment = factory.Create(request);

            Assert.Multiple(() =>
            {
                Assert.That(comment.Content, Is.EqualTo(request.Content));
                Assert.That(comment.Picture, Is.EqualTo(request.Picture));
                Assert.That(comment.CreatorId, Is.EqualTo(request.CreatorId));
                Assert.That(comment.Created.GetTime(), Is.EqualTo(DateTime.Now.GetTime()));
            });
        }

        [Test]
        public void CreateModel_ByComment()
        {
            var factory = new CommentFactory();
            var comment = new Comment
            {
                Id = 1,
                Content = "content",
                Created = new DateTime(1, 1, 1),
                Creator = new ApplicationUser { Id = "xd", UserName = "name", Rank = Rank.Green },
                CreatorId = "xd",
                Picture = "pic",
                Reactions = new List<CommentReaction>()
            };

            var model = factory.CreateModel(comment);

            Assert.Multiple(() =>
            {
                Assert.That(model.HasPicture, Is.True);
                Assert.That(model.Id, Is.EqualTo(comment.Id));
                Assert.That(model.CreatorRank, Is.EqualTo((int)comment.Creator.Rank));
                Assert.That(model.CreatorId, Is.EqualTo(comment.CreatorId));
                Assert.That(model.Content, Is.EqualTo(comment.Content));
                Assert.That(model.Created, Is.EqualTo(comment.Created.GetTime()));
                Assert.That(model.CreatorName, Is.EqualTo(comment.Creator.UserName));
                Assert.That(model.Reactions, Is.EqualTo(comment.Reactions.SumReactions()));
            });
        }

        [Test]
        public void CreateSubcomment_ByArguments()
        {
            var factory = new CommentFactory();
            const int MaincommentId = 1;
            const int SubcommentId = 2;

            var subcomment = factory.CreateSubComment(SubcommentId, MaincommentId);

            Assert.Multiple(() =>
            {
                Assert.That(subcomment.CommentId, Is.EqualTo(SubcommentId));
                Assert.That(subcomment.MainCommentId, Is.EqualTo(MaincommentId));
            });
        }
    }
}
