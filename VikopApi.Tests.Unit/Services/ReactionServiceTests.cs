using Moq;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Reactions;
using VikopApi.Application.Reactions.Abstractions;
using VikopApi.Domain.Enums;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Services
{
    [TestFixture]
    public class ReactionServiceTests
    {
        [Test]
        public async Task AddCommentReaction()
        {
            var reactions = new List<CommentReaction>();

            var managerMock = new Mock<IReactionManager>();
            managerMock.Setup(x => x.AddReaction(It.IsAny<CommentReaction>()))
                .Callback((CommentReaction reaction) => reactions.Add(reaction));

            var factoryMock = new Mock<IReactionFactory>();
            factoryMock.Setup(x => x.CreateCommentReaction(It.IsAny<AddReactionRequest>()))
                .Returns((AddReactionRequest request) => new CommentReaction 
                { 
                    CommentId = request.ObjectId,
                    UserId = request.UserId,
                    Reaction = request.Reaction 
                });

            var service = new ReactionService(factoryMock.Object, managerMock.Object);
            var request = new AddReactionRequest
            {
                ObjectId = 1,
                UserId = "id",
                Reaction = Reaction.Negative,
            };

            await service.AddCommentReaction(request);

            Assert.Multiple(() =>
            {
                Assert.That(reactions.Count(), Is.EqualTo(1));
                Assert.That(reactions.Any(x => x.Reaction == request.Reaction && x.UserId == request.UserId && x.CommentId == request.ObjectId));
            });
        }

        [Test]
        public async Task AddFindingReaction()
        {
            var reactions = new List<FindingReaction>();

            var managerMock = new Mock<IReactionManager>();
            managerMock.Setup(x => x.AddReaction(It.IsAny<FindingReaction>()))
                .Callback((FindingReaction reaction) => reactions.Add(reaction));

            var factoryMock = new Mock<IReactionFactory>();
            factoryMock.Setup(x => x.CreateFindingReaction(It.IsAny<AddReactionRequest>()))
                .Returns((AddReactionRequest request) => new FindingReaction
                {
                    FindingId = request.ObjectId,
                    UserId = request.UserId,
                    Reaction = request.Reaction
                });

            var service = new ReactionService(factoryMock.Object, managerMock.Object);
            var request = new AddReactionRequest
            {
                ObjectId = 1,
                UserId = "id",
                Reaction = Reaction.Negative,
            };

            await service.AddFindingReaction(request);

            Assert.Multiple(() =>
            {
                Assert.That(reactions.Count(), Is.EqualTo(1));
                Assert.That(reactions.Any(x => x.Reaction == request.Reaction && x.UserId == request.UserId && x.FindingId == request.ObjectId));
            });
        }

        [Test]
        public async Task ChangeCommentReaction()
        {
            var reactions = new List<CommentReaction>
            {
                new CommentReaction { UserId = "id", CommentId = 1, Reaction = Reaction.Positive },
                new CommentReaction { UserId = "id2", CommentId = 2, Reaction = Reaction.Positive }
            };

            var managerMock = new Mock<IReactionManager>();
            managerMock.Setup(x => x.ChangeReaction(It.IsAny<CommentReaction>()))
                .Callback((CommentReaction reaction) =>
                {
                    reactions.Remove(reactions.FirstOrDefault(x => x.UserId == reaction.UserId && x.CommentId == reaction.CommentId));
                    reactions.Add(reaction);
                });

            var factoryMock = new Mock<IReactionFactory>();
            factoryMock.Setup(x => x.CreateCommentReaction(It.IsAny<AddReactionRequest>()))
                .Returns((AddReactionRequest request) => new CommentReaction
                {
                    CommentId = request.ObjectId,
                    UserId = request.UserId,
                    Reaction = request.Reaction
                });

            var service = new ReactionService(factoryMock.Object, managerMock.Object);
            var request = new AddReactionRequest
            {
                ObjectId = 1,
                UserId = "id",
                Reaction = Reaction.Negative,
            };

            await service.ChangeCommentReaction(request);

            Assert.Multiple(() =>
            {
                Assert.That(reactions.Count(), Is.EqualTo(2));
                Assert.That(reactions.Any(x => x.Reaction == request.Reaction && x.UserId == request.UserId && x.CommentId == request.ObjectId));
            });
        }

        [Test]
        public async Task ChangeFindingReaction()
        {
            var reactions = new List<FindingReaction>
            {
                new FindingReaction { UserId = "id", FindingId = 1, Reaction = Reaction.Positive },
                new FindingReaction { UserId = "id2", FindingId = 2, Reaction = Reaction.Positive }
            };

            var managerMock = new Mock<IReactionManager>();
            managerMock.Setup(x => x.ChangeReaction(It.IsAny<FindingReaction>()))
                .Callback((FindingReaction reaction) =>
                {
                    reactions.Remove(reactions.FirstOrDefault(x => x.UserId == reaction.UserId && x.FindingId == reaction.FindingId));
                    reactions.Add(reaction);
                });

            var factoryMock = new Mock<IReactionFactory>();
            factoryMock.Setup(x => x.CreateFindingReaction(It.IsAny<AddReactionRequest>()))
                .Returns((AddReactionRequest request) => new FindingReaction
                {
                    FindingId = request.ObjectId,
                    UserId = request.UserId,
                    Reaction = request.Reaction
                });

            var service = new ReactionService(factoryMock.Object, managerMock.Object);
            var request = new AddReactionRequest
            {
                ObjectId = 1,
                UserId = "id",
                Reaction = Reaction.Negative,
            };

            await service.ChangeFindingReaction(request);

            Assert.Multiple(() =>
            {
                Assert.That(reactions.Count(), Is.EqualTo(2));
                Assert.That(reactions.Any(x => x.Reaction == request.Reaction && x.UserId == request.UserId && x.FindingId == request.ObjectId));
            });
        }

        [Test]
        public async Task DeleteCommentReaction()
        {
            var reactions = new List<CommentReaction>
            {
                new CommentReaction { UserId = "id", CommentId = 1, Reaction = Reaction.Positive },
                new CommentReaction { UserId = "id2", CommentId = 2, Reaction = Reaction.Positive }
            };

            var managerMock = new Mock<IReactionManager>();
            managerMock.Setup(x => x.DeleteCommentReaction(It.IsAny<int>(), It.IsAny<string>()))
                .Callback((int commentId, string userId) => reactions.Remove(reactions.FirstOrDefault(x => x.UserId == userId && x.CommentId == commentId)));

            var factoryMock = new Mock<IReactionFactory>();

            var service = new ReactionService(factoryMock.Object, managerMock.Object);
            const int CommentId = 1;
            const string UserId = "id";

            await service.DeleteCommentReaction(CommentId, UserId);

            Assert.Multiple(() =>
            {
                Assert.That(reactions.Count(), Is.EqualTo(1));
                Assert.That(!reactions.Any(x => x.UserId == UserId && x.CommentId == CommentId));
            });
        }

        [Test]
        public async Task DeleteFindingReaction()
        {
            var reactions = new List<FindingReaction>
            {
                new FindingReaction { UserId = "id", FindingId = 1, Reaction = Reaction.Positive },
                new FindingReaction { UserId = "id2", FindingId = 2, Reaction = Reaction.Positive }
            };

            var managerMock = new Mock<IReactionManager>();
            managerMock.Setup(x => x.DeleteFindingReaction(It.IsAny<int>(), It.IsAny<string>()))
                .Callback((int findingId, string userId) => reactions.Remove(reactions.FirstOrDefault(x => x.UserId == userId && x.FindingId == findingId)));

            var factoryMock = new Mock<IReactionFactory>();

            var service = new ReactionService(factoryMock.Object, managerMock.Object);
            const int FindingId = 1;
            const string UserId = "id";

            await service.DeleteFindingReaction(FindingId, UserId);

            Assert.Multiple(() =>
            {
                Assert.That(reactions.Count(), Is.EqualTo(1));
                Assert.That(!reactions.Any(x => x.UserId == UserId && x.FindingId == FindingId));
            });
        }

        [Test]
        public void GetCommentReaction()
        {
            var reactions = new List<CommentReaction>
            {
                new CommentReaction { UserId = "id", CommentId = 1, Reaction = Reaction.Positive },
                new CommentReaction { UserId = "id2", CommentId = 2, Reaction = Reaction.Positive }
            };

            var managerMock = new Mock<IReactionManager>();
            managerMock.Setup(x => x.GetUserCommentReaction(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Func<CommentReaction, Reaction>>()))
                .Returns((string userId, int commentId, Func<CommentReaction, Reaction> selector)
                 => reactions.Where(x => x.UserId == userId && x.CommentId == commentId).Select(selector).FirstOrDefault());

            var factoryMock = new Mock<IReactionFactory>();

            var service = new ReactionService(factoryMock.Object, managerMock.Object);
            const int CommentId = 1;
            const string UserId = "id";

            var res = service.GetCommentReaction(CommentId, UserId);

            Assert.That(res, Is.EqualTo(reactions.Where(x => x.UserId == UserId && x.CommentId == CommentId).Select(x => x.Reaction).FirstOrDefault()));
        }

        [Test]
        public void GetFindingReaction()
        {
            var reactions = new List<FindingReaction>
            {
                new FindingReaction { UserId = "id", FindingId = 1, Reaction = Reaction.Positive },
                new FindingReaction { UserId = "id2", FindingId = 2, Reaction = Reaction.Positive }
            };

            var managerMock = new Mock<IReactionManager>();
            managerMock.Setup(x => x.GetUserFindingReaction(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Func<FindingReaction, Reaction>>()))
                .Returns((string userId, int commentId, Func<FindingReaction, Reaction> selector)
                 => reactions.Where(x => x.UserId == userId && x.FindingId == commentId).Select(selector).FirstOrDefault());

            var factoryMock = new Mock<IReactionFactory>();

            var service = new ReactionService(factoryMock.Object, managerMock.Object);
            const int FindingId = 1;
            const string UserId = "id";

            var res = service.GetFindingReaction(FindingId, UserId);

            Assert.That(res, Is.EqualTo(reactions.Where(x => x.UserId == UserId && x.FindingId == FindingId).Select(x => x.Reaction).FirstOrDefault()));
        }
    }
}
