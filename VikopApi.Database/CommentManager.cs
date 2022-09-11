using Microsoft.EntityFrameworkCore;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Database
{
    public class CommentManager : ICommentManager
    {
        private readonly AppDbContext _dbContext;

        public CommentManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddComment(Comment comment)
        {
            _dbContext.Comments.Add(comment);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddFindingComment(int commentId, int findingId)
        {
            _dbContext.FindingComments.Add(new FindingComment 
            { 
                CommentId = commentId,
                FindingId = findingId 
            });

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public T GetCommentById<T>(int id, Func<Comment, T> selector)
        => _dbContext.Comments.Include(comment => comment.Creator)
            .Where(comment => comment.Id == id)
            .Select(selector).FirstOrDefault();
    }
}
