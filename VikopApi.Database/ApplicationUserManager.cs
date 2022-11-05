using Microsoft.EntityFrameworkCore;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;
using VikopApi.Domain.Enums;

namespace VikopApi.Database
{
    [Implementation(typeof(IApplicationUserManager))]
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
                .Include(finding => finding.Tags)
                .ThenInclude(tag => tag.Tag)
                .Where(finding => finding.CreatorId == userId)
                .OrderByDescending(finding => finding.Created)
                .Select(selector);

        public IEnumerable<T> GetUserPosts<T>(string userId, Func<Post, T> selector)
            => _dbContext.Posts
                .Include(post => post.Comment)
                .ThenInclude(comment => comment.Creator)
                .Include(post => post.Comment)
                .ThenInclude(comment => comment.Reactions)
                .Include(post => post.Tags)
                .ThenInclude(tag => tag.Tag)
                .Where(post => post.Comment.CreatorId == userId)
                .OrderByDescending(post => post.Comment.Created)
                .Select(selector);

        public IEnumerable<T> GetUsers<T>(Func<ApplicationUser, T> selector)
            => _dbContext.Users.Select(selector);

        public async Task<bool> UpdateRanks()
        {
            var users = _dbContext.Users.Where(user => (int)user.Rank < 2);

            await users.ForEachAsync(user =>
            {
                var timeSinceCreation = DateTime.Now - user.Created;

                if (timeSinceCreation.Days > 30 && user.Rank == Rank.Green)
                {
                    user.Rank = Rank.Orange;
                }
                else if(timeSinceCreation.Days > 365 && user.Rank == Rank.Orange)
                {
                    user.Rank = Rank.Red;
                }
            });

            // added because SaveChangesAsync will be negative if there are no changes in context
            if(!_dbContext.ChangeTracker.Entries()
                .Any(entity => entity.State == EntityState.Modified))
            {
                return true;
            }

            return await _dbContext.SaveChangesAsync() > 0;
        }

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
