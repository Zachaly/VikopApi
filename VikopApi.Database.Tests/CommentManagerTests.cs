using Microsoft.EntityFrameworkCore;

namespace VikopApi.Database.Tests
{
    public class CommentManagerTests : DatabaseTest
    {
        private readonly ICommentManager _commentManager;

        public CommentManagerTests() : base()
        {
            _commentManager = new CommentManager(_dbContext);
        }

        private int SumReactions(IEnumerable<CommentReaction> reactions)
            => reactions.Sum(reaction => (int)reaction.Reaction);

        [Fact]
        public void Get_Comment_By_Id()
        {
            var comment = _commentManager.GetCommentById(5, x => x);

            Assert.Equal(5, comment.Id);
            Assert.Equal("comment5", comment.Content);
            Assert.Equal("3", comment.CreatorId);
            Assert.Equal(3, comment.Reactions.Count());
            Assert.Equal(-1, SumReactions(comment.Reactions));
        }

        [Fact]
        public void Get_Comment_By_Invalid_Id()
        {
            var comment = _commentManager.GetCommentById(2137, x => x);

            Assert.Null(comment);
        }

        [Fact]
        public async Task Add_Comment()
        {
            var comment = new Comment
            {
                Id = 2137,
                Content = "new comment",
                Created = DateTime.Now,
                CreatorId = "1"
            };

            var res = await _commentManager.AddComment(comment);

            var user = _dbContext.Users.Include(user => user.Comments).FirstOrDefault(user => user.Id == "1");

            Assert.True(res);
            Assert.Contains(_dbContext.Comments, comment => comment.Id == 2137);
            Assert.Contains(_dbContext.Comments, comment => comment.Id == 2137 && comment.Content == "new comment");
            Assert.Contains(user?.Comments, comment => comment.Id == 2137);
        }

        [Fact]
        public async Task Add_Finding_Comment()
        {
            var comment = new Comment
            {
                Id = 2137,
                Content = "new comment",
                Created = DateTime.Now,
                CreatorId = "1"
            };

            _dbContext.Comments.Add(comment);
            await _dbContext.SaveChangesAsync();

            var findingComment = new FindingComment { CommentId = 2137, FindingId = 1 };

            var res = await _commentManager.AddFindingComment(2137, 1);

            var finding = _dbContext.Findings.Include(finding => finding.Comments).ThenInclude(comment => comment.Comment)
                .FirstOrDefault(finding => finding.Id == 1);

            Assert.True(res);
            Assert.Contains(finding?.Comments, comment => comment.CommentId == 2137);
            Assert.Contains(finding?.Comments, comment => comment.Comment.Content == "new comment");
        }

        [Fact]
        public async Task Add_Invalid_Comment()
        {
            var comment = new Comment();

            await Assert.ThrowsAsync<DbUpdateException>(async () => await _commentManager.AddComment(comment));
        }

        [Fact]
        public async Task Add_Invalid_Finding_Comment()
        {
            await Assert.ThrowsAsync<DbUpdateException>(async () => await _commentManager.AddFindingComment(21, 37));
        }

        [Fact]
        public async Task Add_Reaction()
        {
            var reaction = new CommentReaction
            {
                CommentId = 1,
                UserId = "1",
                Reaction = Reaction.Positive
            };

            var res = await _commentManager.AddReaction(reaction);

            var comment = _dbContext.Comments.Include(x => x.Reactions).FirstOrDefault(x => x.Id == 1);

            Assert.True(res);
            Assert.Equal(2, comment.Reactions.Count());
            Assert.Equal(0, SumReactions(comment.Reactions));
        }

        [Fact]
        public async Task Add_Existent_Reaction()
        {
            var reaction = new CommentReaction
            {
                CommentId = 1,
                UserId = "2",
                Reaction = Reaction.Positive
            };

            var res = await _commentManager.AddReaction(reaction);

            var comment = _dbContext.Comments.Include(x => x.Reactions).FirstOrDefault(x => x.Id == 1);

            Assert.False(res);
            Assert.Single(comment.Reactions);
            Assert.Equal(-1, SumReactions(comment.Reactions));
        }

        [Fact]
        public async Task Change_Reaction()
        {
            var reaction = new CommentReaction
            {
                CommentId = 5,
                UserId = "3",
                Reaction = Reaction.Positive
            };

            var res = await _commentManager.ChangeReaction(reaction);

            var comment = _dbContext.Comments.Include(x => x.Reactions).FirstOrDefault(x => x.Id == 5);

            Assert.True(res);
            Assert.Equal(3, comment.Reactions.Count());
            Assert.Equal(1, SumReactions(comment.Reactions));
        }

        [Fact]
        public async Task Change_Nonexistent_Reaction()
        {
            var reaction = new CommentReaction
            {
                CommentId = 1,
                UserId = "3",
                Reaction = Reaction.Positive
            };

            var res = await _commentManager.ChangeReaction(reaction);

            var comment = _dbContext.Comments.Include(x => x.Reactions).FirstOrDefault(x => x.Id == 1);

            Assert.False(res);
            Assert.Single(comment.Reactions);
            Assert.Equal(-1, SumReactions(comment.Reactions));
        }

        [Fact]
        public async Task Delete_Reaction()
        {
            var res = await _commentManager.DeleteReaction(1, "2");

            var comment = _dbContext.Comments.Include(x => x.Reactions).FirstOrDefault(x => x.Id == 1);

            Assert.True(res);
            Assert.Empty(comment.Reactions);
        }

        [Fact]
        public async Task Delete_Nonexistent_Reaction()
        {
            var res = await _commentManager.DeleteReaction(1, "1");

            var comment = _dbContext.Comments.Include(x => x.Reactions).FirstOrDefault(x => x.Id == 1);

            Assert.True(res);
            Assert.Single(comment.Reactions);
        }
    }
}
