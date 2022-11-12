using Microsoft.EntityFrameworkCore;
using VikopApi.Database.Migrations;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Database
{
    [Implementation(typeof(IReportManager))]
    public class ReportManager : IReportManager
    {
        private readonly AppDbContext _dbContext;

        public ReportManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddReport(FindingReport report)
        {
            _dbContext.FindingReports.Add(report);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddReport(PostReport report)
        {
            _dbContext.PostReports.Add(report);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public IEnumerable<T> GetReports<T>(int pageIndex, int pageSize, Func<PostReport, T> selector)
            => _dbContext.PostReports
                .Include(report => report.ReportingUser)
                .Include(report => report.Post)
                .OrderByDescending(report => report.Created)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(selector);

        public IEnumerable<T> GetReports<T>(int pageIndex, int pageSize, Func<FindingReport, T> selector)
            => _dbContext.FindingReports
                .Include(report => report.ReportingUser)
                .Include(report => report.Finding)
                .OrderByDescending(report => report.Created)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(selector);

        public async Task<bool> RemoveFindingReport(int findingId)
        {
            var reports = _dbContext.FindingReports.Where(report => report.FindingId == findingId);

            _dbContext.FindingReports.RemoveRange(reports);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemovePostReport(int postId)
        {
            var reports = _dbContext.PostReports.Where(report => report.PostId == postId);

            _dbContext.PostReports.RemoveRange(reports);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public T GetReportById<T>(int id, Func<FindingReport, T> selector)
            => _dbContext.FindingReports
                .Include(report => report.ReportingUser)
                .Include(report => report.Finding)
                .ThenInclude(finding => finding.Creator)
                .Include(report => report.Finding)
                .ThenInclude(finding => finding.Tags)
                .Include(report => report.Finding)
                .ThenInclude(finding => finding.Comments)
                .Include(report => report.Finding)
                .ThenInclude(finding => finding.Reactions)
                .Include(report => report.Finding)
                .ThenInclude(finding => finding.Tags)
                .ThenInclude(tag => tag.Tag)
                .Where(report => report.Id == id)
                .Select(selector)
                .FirstOrDefault();

        public T GetReportById<T>(int id, Func<PostReport, T> selector)
            => _dbContext.PostReports
                .Include(report => report.ReportingUser)
                .Include(report => report.Post)
                .ThenInclude(post => post.Comment)
                .ThenInclude(comment => comment.Creator)
                .Include(report => report.Post)
                .ThenInclude(post => post.Comment)
                .ThenInclude(comment => comment.Reactions)
                .Include(report => report.Post)
                .ThenInclude(post => post.Tags)
                .Where(report => report.Id == id)
                .Select(selector)
                .FirstOrDefault();
    }
}
