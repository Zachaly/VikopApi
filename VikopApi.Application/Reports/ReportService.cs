using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Reports.Abstractions;

namespace VikopApi.Application.Reports
{
    [Implementation(typeof(IReportService))]
    public class ReportService : IReportService
    {
        private IReportManager _reportManager;
        private IReportFactory _reportFactory;

        public ReportService(IReportManager reportManager, IReportFactory reportFactory)
        {
            _reportManager = reportManager;
            _reportFactory = reportFactory;
        }

        public Task<bool> AddFindingReport(AddReportRequest request)
        {
            var report = _reportFactory.CreateFinding(request);

            return _reportManager.AddReport(report);
        }

        public Task<bool> AddPostReport(AddReportRequest request)
        {
            var report = _reportFactory.CreatePost(request);

            return _reportManager.AddReport(report);
        }

        public FindingReportModel GetFindingReportById(int id)
            => _reportManager.GetReportById(id, (FindingReport report) => _reportFactory.CreateModel(report));

        public IEnumerable<ReportListItem> GetFindingReports(int? pageIndex, int? pageSize)
        {
            var index = pageIndex ?? 0;
            var size = pageSize ?? 10;

            return _reportManager.GetReports(index, size, (FindingReport report) => _reportFactory.CreateListItem(report));
        }

        public PostReportModel GetPostReportById(int id)
            => _reportManager.GetReportById(id, (PostReport report) => _reportFactory.CreateModel(report));

        public IEnumerable<ReportListItem> GetPostReports(int? pageIndex, int? pageSize)
        {
            var index = pageIndex ?? 0;
            var size = pageSize ?? 10;

            return _reportManager.GetReports(index, size, (PostReport report) => _reportFactory.CreateListItem(report));
        }

        public Task<bool> RemoveFindingReport(int findingId)
        {
            try
            {
                return _reportManager.RemoveFindingReport(findingId);
            }
            catch(Exception)
            {
                return Task.FromResult(false);
            }
        }

        public Task<bool> RemovePostReport(int postId)
        {
            try
            {
                return _reportManager.RemovePostReport(postId);
            }
            catch (Exception)
            {
                return Task.FromResult(false);
            }
        }
    }
}
