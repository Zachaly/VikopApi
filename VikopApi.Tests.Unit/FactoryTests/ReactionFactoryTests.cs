using VikopApi.Application.Models.Requests;
using VikopApi.Application.Reactions;
using VikopApi.Domain.Enums;

namespace VikopApi.Tests.Unit.FactoryTests
{
    [TestFixture]
    public class ReactionFactoryTests
    {
        [Test]
        public void CreateFindingReaction()
        {
            var factory = new ReactionFactory();
            var request = new AddReactionRequest
            {
                ObjectId = 1,
                Reaction = Reaction.Positive,
                UserId = "id"
            };

            var reaction = factory.CreateFindingReaction(request);

            Assert.Multiple(() =>
            {
                Assert.That(reaction.Reaction, Is.EqualTo(request.Reaction));
                Assert.That(reaction.UserId, Is.EqualTo(request.UserId));
                Assert.That(reaction.FindingId, Is.EqualTo(request.ObjectId));
            });
        }

        [Test]
        public void CreateCommentReaction()
        {
            var factory = new ReactionFactory();
            var request = new AddReactionRequest
            {
                ObjectId = 1,
                Reaction = Reaction.Positive,
                UserId = "id"
            };

            var reaction = factory.CreateCommentReaction(request);

            Assert.Multiple(() =>
            {
                Assert.That(reaction.Reaction, Is.EqualTo(request.Reaction));
                Assert.That(reaction.UserId, Is.EqualTo(request.UserId));
                Assert.That(reaction.CommentId, Is.EqualTo(request.ObjectId));
            });
        }
    }
}
