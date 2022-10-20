using Microsoft.EntityFrameworkCore;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Database
{
    public class FindingManager : IFindingManager
    {
        private AppDbContext _dbContext;

        public FindingManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddFinding(Finding finding)
        {
            _dbContext.Findings.Add(finding);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddReaction(FindingReaction reaction)
        {
            if (GetReaction(reaction.UserId, reaction.FindingId) != null)
            {
                return false;
            }

            _dbContext.FindingReactions.Add(reaction);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        private FindingReaction GetReaction(string userId, int findingId)
            => _dbContext.FindingReactions
                .FirstOrDefault(reaction => reaction.FindingId == findingId && reaction.UserId == userId);

        public async Task<bool> ChangeReaction(FindingReaction newReaction)
        {
            var originalReaction = GetReaction(newReaction.UserId, newReaction.FindingId);

            if (originalReaction is null)
            {
                return false;
            }

            originalReaction.Reaction = newReaction.Reaction;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteReaction(int findingId, string userId)
        {
            var reaction = GetReaction(userId, findingId);

            if (reaction is null)
            {
                return true;
            }

            _dbContext.FindingReactions.Remove(reaction);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public T GetFindingById<T>(int id, Func<Finding, T> selector)
            => _dbContext.Findings.Include(finding => finding.Creator)
                .Include(finding => finding.Reactions)
                .Include(finding => finding.Comments)
                .ThenInclude(comment => comment.Comment)
                .ThenInclude(comment => comment.Creator)
                .Include(finding => finding.Comments)
                .ThenInclude(comment => comment.Comment)
                .ThenInclude(comment => comment.Reactions)
                .Include(finding => finding.Tags)
                .ThenInclude(tag => tag.Tag)
                .Where(finding => finding.Id == id)
                .Select(selector).FirstOrDefault();

        public IEnumerable<T> GetAllFindings<T>(int pageIndex, int pageSize, Func<Finding, T> selector)
            => _dbContext.Findings.Include(finding => finding.Creator)
                .Include(finding => finding.Comments)
                .Include(finding => finding.Reactions)
                .Include(finding => finding.Tags)
                .ThenInclude(tag => tag.Tag)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(selector);

        public T GetUserReaction<T>(int findingId, string userId, Func<FindingReaction, T> selector)
            => _dbContext.FindingReactions
                .Where(reaction => reaction.UserId == userId && reaction.FindingId == findingId)
                .Select(selector)
                .FirstOrDefault();

        public IEnumerable<T> GetNewFindings<T>(int pageIndex, int pageSize, Func<Finding, T> selector)
             => _dbContext.Findings.Include(finding => finding.Creator)
                .Include(finding => finding.Comments)
                .Include(finding => finding.Reactions)
                .Include(finding => finding.Tags)
                .ThenInclude(tag => tag.Tag)
                .OrderByDescending(finding => finding.Created)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(selector);

        // many things need to be included if finding value is determined also by comments
        public IEnumerable<T> GetTopFindings<T>(int pageIndex, int pageSize, Func<Finding, T> selector)
            => _dbContext.Findings.Include(finding => finding.Creator)
                .Include(finding => finding.Comments)
                .ThenInclude(comment => comment.Comment)
                .ThenInclude(comment => comment.SubComments)
                .ThenInclude(subcomment => subcomment.Comment)
                .ThenInclude(subcomment => subcomment.Reactions)
                .Include(finding => finding.Comments)
                .ThenInclude(comment => comment.Comment)
                .ThenInclude(comment => comment.Reactions)
                .Include(finding => finding.Reactions)
                .Include(finding => finding.Tags)
                .ThenInclude(tag => tag.Tag)
                .AsEnumerable()
                .OrderByDescending(finding => finding.FindingValue())
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(selector);

        public int GetPageCount(int pageSize)
            => (int)Math.Ceiling(_dbContext.Findings.Count() / (decimal)pageSize);
    }
}
