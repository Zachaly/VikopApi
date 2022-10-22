using Microsoft.EntityFrameworkCore;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Database
{
    [Implementation(typeof(ICommentManager))]
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
            if(!_dbContext.Findings.Any(finding => finding.Id == findingId) || 
                !_dbContext.Comments.Any(comment => comment.Id == commentId))
            {
                throw new DbUpdateException("Comment or finding with given id does not exists!");
            }

            _dbContext.FindingComments.Add(new FindingComment 
            { 
                CommentId = commentId,
                FindingId = findingId 
            });

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public T GetCommentById<T>(int id, Func<Comment, T> selector)
            => _dbContext.Comments.Include(comment => comment.Creator)
                .Include(comment => comment.Reactions)
                .Where(comment => comment.Id == id)
                .Select(selector).FirstOrDefault();

        public async Task<bool> AddSubComment(SubComment subComment)
        {
            _dbContext.SubComments.Add(subComment);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public IEnumerable<T> GetSubComments<T>(int mainCommentId, Func<SubComment, T> selector)
            => _dbContext.Comments
                .Include(comment => comment.SubComments)
                .ThenInclude(subcomment => subcomment.Comment)
                .ThenInclude(comment => comment.Creator)
                .Include(comment => comment.SubComments)
                .ThenInclude(subcomment => subcomment.Comment)
                .ThenInclude(comment => comment.Reactions)
                .FirstOrDefault(comment => comment.Id == mainCommentId)?
                .SubComments.Select(selector);
    }
}
