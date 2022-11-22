using VikopApi.Application.Models;
using VikopApi.Application.Models.Report;
using VikopApi.Application.Models.Report.Requests;

namespace VikopApi.Application.Reports.Abstractions
{
    public interface IReportService
    {
        Task<bool> AddPostReport(AddReportRequest request);
        Task<bool> AddFindingReport(AddReportRequest request);
        Task<bool> RemoveFindingReport(int findingId);
        Task<bool> RemovePostReport(int postId);
        IEnumerable<ReportListItem> GetPostReports(PagedRequest request);
        IEnumerable<ReportListItem> GetFindingReports(PagedRequest request);
        PostReportModel GetPostReportById(int id);
        FindingReportModel GetFindingReportById(int id);
    }
}
