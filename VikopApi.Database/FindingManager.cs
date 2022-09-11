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

        public T GetFindingById<T>(int id, Func<Finding, T> selector)
            => _dbContext.Findings.Include(finding => finding.Creator)
                .Include(finding => finding.Comments)
                .ThenInclude(comment => comment.Comment)
                .ThenInclude(comment => comment.Creator)
                .Where(finding => finding.Id == id)
                .Select(selector).FirstOrDefault();

        public IEnumerable<T> GetFindings<T>(Func<Finding, T> selector)
            => _dbContext.Findings.Include(finding => finding.Creator)
                .Include(finding => finding.Comments)
                .Select(selector);
    }
}
