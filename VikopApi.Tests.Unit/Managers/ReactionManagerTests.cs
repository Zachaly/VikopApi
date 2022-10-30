using VikopApi.Database;
using VikopApi.Domain.Enums;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Managers
{
    [TestFixture]
    public class ReactionManagerTests
    {
        [Test]
        [TestCase(1, "id1")]
        [TestCase(1, "id2")]
        [TestCase(1, "id3")]
        [TestCase(2, "id1")]
        [TestCase(2, "id2")]
        [TestCase(2, "id3")]
        [TestCase(3, "id1")]
        public void GetUserCommentReaction(int commentId, string userId)
        {
            var commentReactions = new List<CommentReaction>
            {
                new CommentReaction { CommentId = 1, UserId = "id1"},
                new CommentReaction { CommentId = 1, UserId = "id2"},
                new CommentReaction { CommentId = 2, UserId = "id3"},
                new CommentReaction { CommentId = 2, UserId = "id1"},
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(commentReactions);
            var manager = new ReactionManager(dbContext);

            var res = manager.GetUserCommentReaction(userId, commentId, x => x);

            Assert.That(res, Is.EqualTo(commentReactions.FirstOrDefault(x => x.CommentId == commentId && x.UserId == userId)));
        }

        [Test]
        [TestCase(1, "id1")]
        [TestCase(1, "id2")]
        [TestCase(1, "id3")]
        [TestCase(2, "id1")]
        [TestCase(2, "id2")]
        [TestCase(2, "id3")]
        [TestCase(3, "id1")]
        public void GetUserFindingReaction(int commentId, string userId)
        {
            var commentReactions = new List<FindingReaction>
            {
                new FindingReaction { FindingId = 1, UserId = "id1"},
                new FindingReaction { FindingId = 1, UserId = "id2"},
                new FindingReaction { FindingId = 2, UserId = "id3"},
                new FindingReaction { FindingId = 2, UserId = "id1"},
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(commentReactions);
            var manager = new ReactionManager(dbContext);

            var res = manager.GetUserFindingReaction(userId, commentId, x => x);

            Assert.That(res, Is.EqualTo(commentReactions.FirstOrDefault(x => x.FindingId == commentId && x.UserId == userId)));
        }

        [Test]
        public async Task AddReaction_Comment_Success()
        {
            var dbContext = Extensions.GetAppDbContext();
            var manager = new ReactionManager(dbContext);
            var reaction = new CommentReaction { CommentId = 1, UserId = "id1" };

            var res = await manager.AddReaction(reaction);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(dbContext.CommentReactions.Contains(reaction));
            });
        }

        [Test]
        public async Task AddReaction_Comment_ReactionExists_Fail()
        {
            var dbContext = Extensions.GetAppDbContext();
            var manager = new ReactionManager(dbContext);
            var reaction = new CommentReaction { CommentId = 1, UserId = "id1" };
            dbContext.AddContent(new List<CommentReaction> { new CommentReaction { CommentId = 1, UserId = "id1" } });

            var res = await manager.AddReaction(reaction);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.False);
                Assert.That(dbContext.CommentReactions.Count(), Is.EqualTo(1));
            });
        }

        [Test]
        public async Task AddReaction_Finding_Success()
        {
            var dbContext = Extensions.GetAppDbContext();
            var manager = new ReactionManager(dbContext);
            var reaction = new FindingReaction { FindingId = 1, UserId = "id1" };

            var res = await manager.AddReaction(reaction);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(dbContext.FindingReactions.Contains(reaction));
            });
        }

        [Test]
        public async Task AddReaction_Finding_ReactionExists_Fail()
        {
            var dbContext = Extensions.GetAppDbContext();
            var manager = new ReactionManager(dbContext);
            var reaction = new FindingReaction { FindingId = 1, UserId = "id1" };
            dbContext.AddContent(new List<FindingReaction> { new FindingReaction { FindingId = 1, UserId = "id1" } });

            var res = await manager.AddReaction(reaction);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.False);
                Assert.That(dbContext.FindingReactions.Count(), Is.EqualTo(1));
            });
        }

        [Test]
        public async Task ChangeReaction_Comment_Success()
        {
            var reaction = new CommentReaction { CommentId = 1, UserId = "id1", Reaction = Reaction.Positive };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(new List<CommentReaction> { reaction });
            var manager = new ReactionManager(dbContext);

            var res = await manager.ChangeReaction(new CommentReaction { CommentId = 1, UserId = "id1", Reaction = Reaction.Negative });

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(reaction.Reaction, Is.EqualTo(Reaction.Negative));
            });
        }

        [Test]
        public async Task ChangeReaction_Comment_NonexistentReacion_Fails()
        {
            var reaction = new CommentReaction { CommentId = 1, UserId = "id1", Reaction = Reaction.Positive };
            var dbContext = Extensions.GetAppDbContext();
            var manager = new ReactionManager(dbContext);

            var res = await manager.ChangeReaction(reaction);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.False);
                Assert.That(dbContext.CommentReactions.Count(), Is.EqualTo(0));
            });
        }

        [Test]
        public async Task ChangeReaction_Finding_Success()
        {
            var reaction = new FindingReaction { FindingId = 1, UserId = "id1", Reaction = Reaction.Positive };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(new List<FindingReaction> { reaction });
            var manager = new ReactionManager(dbContext);

            var res = await manager.ChangeReaction(new FindingReaction { FindingId = 1, UserId = "id1", Reaction = Reaction.Negative });

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(reaction.Reaction, Is.EqualTo(Reaction.Negative));
            });
        }

        [Test]
        public async Task ChangeReaction_Finding_NonexistentReacion_Fails()
        {
            var reaction = new FindingReaction { FindingId = 1, UserId = "id1", Reaction = Reaction.Positive };
            var dbContext = Extensions.GetAppDbContext();
            var manager = new ReactionManager(dbContext);

            var res = await manager.ChangeReaction(reaction);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.False);
                Assert.That(dbContext.FindingReactions.Count(), Is.EqualTo(0));
            });
        }

        [Test]
        public async Task DeleteCommentReaction_ReactionExists_Success()
        {
            var reaction = new CommentReaction { CommentId = 1, UserId = "id" };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(new List<CommentReaction> { reaction });
            var manager = new ReactionManager(dbContext);

            var res = await manager.DeleteCommentReaction(reaction.CommentId, reaction.UserId);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(!dbContext.CommentReactions.Contains(reaction));
            });
        }

        [Test]
        public async Task DeleteCommentReaction_ReactionDoesNotExists_Success()
        {
            var reaction = new CommentReaction { CommentId = 1, UserId = "id" };
            var dbContext = Extensions.GetAppDbContext();
            var manager = new ReactionManager(dbContext);

            var res = await manager.DeleteCommentReaction(reaction.CommentId, reaction.UserId);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(!dbContext.CommentReactions.Contains(reaction));
            });
        }

        [Test]
        public async Task DeleteFindingReaction_ReactionExists_Success()
        {
            var reaction = new FindingReaction { FindingId = 1, UserId = "id" };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(new List<FindingReaction> { reaction });
            var manager = new ReactionManager(dbContext);

            var res = await manager.DeleteFindingReaction(reaction.FindingId, reaction.UserId);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(!dbContext.FindingReactions.Contains(reaction));
            });
        }

        [Test]
        public async Task DeleteFindingReaction_ReactionDoesNotExists_Success()
        {
            var reaction = new FindingReaction { FindingId = 1, UserId = "id" };
            var dbContext = Extensions.GetAppDbContext();
            var manager = new ReactionManager(dbContext);

            var res = await manager.DeleteFindingReaction(reaction.FindingId, reaction.UserId);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(!dbContext.FindingReactions.Contains(reaction));
            });
        }
    }
}
