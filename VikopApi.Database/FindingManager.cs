using Microsoft.EntityFrameworkCore;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Database
{
    [Implementation(typeof(IFindingManager))]
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

        public IEnumerable<T> SearchFindings<T>(int pageIndex, int pageSize, IEnumerable<Func<Finding, bool>> conditions, Func<Finding, T> selector)
            => _dbContext.Findings.Include(finding => finding.Creator)
                .Include(finding => finding.Comments)
                .Include(finding => finding.Reactions)
                .Include(finding => finding.Tags)
                .ThenInclude(tag => tag.Tag)
                .OrderByDescending(finding => finding.Created)
                .AsEnumerable()
                .Where(finding => conditions.All(condition => condition(finding)))
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(selector);

        public async Task<bool> RemoveFindingById(int id)
        {
            var finding = _dbContext.Findings
                .Include(finding => finding.Comments)
                .ThenInclude(comment => comment.Comment)
                .ThenInclude(comment => comment.SubComments)
                .FirstOrDefault(finding => finding.Id == id);

            _dbContext.Findings.Remove(finding);

            var subComments = finding.Comments
                .Select(comment => comment.Comment)
                .SelectMany(comment => comment.SubComments)
                .Select(comment => comment.Comment);

            _dbContext.Comments.RemoveRange(subComments);
            _dbContext.Comments.RemoveRange(finding.Comments.Select(comment => comment.Comment));

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
