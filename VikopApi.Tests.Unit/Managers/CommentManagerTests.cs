using Microsoft.EntityFrameworkCore;
using VikopApi.Database;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Managers
{
    [TestFixture]
    public class CommentManagerTests
    {
        [Test]
        public async Task AddComment()
        {
            var comment = new Comment { Id = 1 };
            var dbContext = Extensions.GetAppDbContext();
            var manager = new CommentManager(dbContext);
            
            var res = await manager.AddComment(comment);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(dbContext.Comments.Count(), Is.EqualTo(1));
                Assert.That(dbContext.Comments.Any(x => x.Id == comment.Id));
            });
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(3, 4)]
        public async Task AddFindingComment_Success(int commentId, int findingId)
        {
            var comments = new List<Comment> { new Comment { Id = 1 }, new Comment { Id = 3 } };
            var findings = new List<Finding> { new Finding { Id = 2 }, new Finding { Id = 4 } };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(comments);
            dbContext.AddContent(findings);
            var manager = new CommentManager(dbContext);

            var res = await manager.AddFindingComment(commentId, findingId);

            Assert.Multiple(() =>
            {
                Assert.That(dbContext.FindingComments.Any(comment => 
                    comment.CommentId == commentId 
                    && comment.FindingId == findingId),
                    Is.True);
                Assert.That(dbContext.FindingComments.Count(), Is.EqualTo(1));
            });
        }

        [Test]
        public void AddFindingComment_InvalidCommentId_ThrowsError()
        {
            var comments = new List<Comment> { new Comment { Id = 1 }, new Comment { Id = 3 } };
            var findings = new List<Finding> { new Finding { Id = 2 }, new Finding { Id = 4 } };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(comments);
            dbContext.AddContent(findings);
            var manager = new CommentManager(dbContext);

            Assert.ThrowsAsync<DbUpdateException>(async () => await manager.AddFindingComment(5, 2));
        }

        [Test]
        public void AddFindingComment_InvalidFindingId_ThrowsError()
        {
            var comments = new List<Comment> { new Comment { Id = 1 }, new Comment { Id = 3 } };
            var findings = new List<Finding> { new Finding { Id = 2 }, new Finding { Id = 4 } };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(comments);
            dbContext.AddContent(findings);
            var manager = new CommentManager(dbContext);

            Assert.ThrowsAsync<DbUpdateException>(async () => await manager.AddFindingComment(1, 5));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void GetCommentById(int commentId)
        {
            var comments = new List<Comment>
            {
                new Comment { Id = 1 },
                new Comment { Id = 2 },
                new Comment { Id = 3 }
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(comments);
            var manager = new CommentManager(dbContext);

            var comment = manager.GetCommentById(commentId, x => x);

            Assert.That(comment, Is.EqualTo(comments.FirstOrDefault(x => x.Id == commentId)));
        }

        [Test]
        public async Task AddSubcomment()
        {
            var subcomment = new SubComment { CommentId = 1, MainCommentId = 2 };
            var dbContext = Extensions.GetAppDbContext();
            var manager = new CommentManager(dbContext);

            var res = await manager.AddSubComment(subcomment);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(dbContext.SubComments.Any(comment => 
                    comment.CommentId == subcomment.CommentId 
                    && comment.MainCommentId == subcomment.MainCommentId),
                    Is.True);
            });
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetSubcomments_ReturnsSubcomments(int commentId)
        {
            var comments = new List<Comment>
            {
                new Comment { Id = 1 },
                new Comment { Id = 2 },
                new Comment { Id = 3 }
            };
            var subcomments = new List<SubComment>
            {
                new SubComment { MainCommentId = 1, CommentId = 10 },
                new SubComment { MainCommentId = 2, CommentId = 11 },
                new SubComment { MainCommentId = 3, CommentId = 12 },
                new SubComment { MainCommentId = 1, CommentId = 13 },
                new SubComment { MainCommentId = 2, CommentId = 14 },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(comments);
            dbContext.AddContent(subcomments);
            var manager = new CommentManager(dbContext);

            var result = manager.GetSubComments(commentId, x => x);

            Assert.That(result, Is.EquivalentTo(subcomments.Where(x => x.MainCommentId == commentId)));
        }

        [Test]
        public void GetSubcomments_ReturnsNull()
        {
            var comments = new List<Comment>
            {
                new Comment { Id = 1 },
                new Comment { Id = 2 },
                new Comment { Id = 3 }
            };
            var subcomments = new List<SubComment>
            {
                new SubComment { MainCommentId = 1, CommentId = 10 },
                new SubComment { MainCommentId = 2, CommentId = 11 },
                new SubComment { MainCommentId = 3, CommentId = 12 },
                new SubComment { MainCommentId = 1, CommentId = 13 },
                new SubComment { MainCommentId = 2, CommentId = 14 },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(comments);
            dbContext.AddContent(subcomments);
            var manager = new CommentManager(dbContext);

            var result = manager.GetSubComments(4, x => x);

            Assert.That(result, Is.Null);
        }
    }
}
