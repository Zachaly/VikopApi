using Microsoft.EntityFrameworkCore;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Database
{
    public class ApplicationUserManager : IApplicationUserManager
    {
        private readonly AppDbContext _dbContext;

        public ApplicationUserManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetUserById<T>(string id, Func<ApplicationUser, T> selector)
            => _dbContext.Users
                .Where(user => user.Id == id)
                .Select(selector)
                .FirstOrDefault();

        public IEnumerable<T> GetUserFindings<T>(string userId, Func<Finding, T> selector)
            => _dbContext.Findings
                .Include(finding => finding.Reactions)
                .Include(finding => finding.Creator)
                .Include(finding => finding.Comments)
                .Where(finding => finding.CreatorId == userId)
                .Select(selector);

        public IEnumerable<T> GetUserPosts<T>(string userId, Func<Post, T> selector)
            => _dbContext.Posts
                .Include(post => post.Comment)
                .ThenInclude(comment => comment.Creator)
                .Include(post => post.Comment)
                .ThenInclude(comment => comment.Reactions)
                .Where(post => post.Comment.CreatorId == userId)
                .Select(selector);

        public IEnumerable<T> GetUsers<T>(Func<ApplicationUser, T> selector)
            => _dbContext.Users.Select(selector);

        public async Task<bool> UpdateUser(string userId, Action<ApplicationUser> changes)
        {
            var user = _dbContext.Users.FirstOrDefault(user => user.Id == userId);

            if(user is null)
            {
                throw new DbUpdateException("User does not exist");
            }

            changes(user);

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
