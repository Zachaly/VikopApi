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

        [Fact]
        public void Get_Comment_By_Id()
        {
            var comment = _commentManager.GetCommentById(5, x => x);

            Assert.Equal(5, comment.Id);
            Assert.Equal("comment5", comment.Content);
            Assert.Equal("3", comment.CreatorId);
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
    }
}
