using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.Reports.Abstractions
{
    public interface IReportService
    {
        Task<bool> AddPostReport(AddReportRequest request);
        Task<bool> AddFindingReport(AddReportRequest request);
        Task<bool> RemoveFindingReport(int findingId);
        Task<bool> RemovePostReport(int postId);
        IEnumerable<ReportListItem> GetPostReports(int? pageIndex, int? pageSize);
        IEnumerable<ReportListItem> GetFindingReports(int? pageIndex, int? pageSize);
        PostReportModel GetPostReportById(int id);
        FindingReportModel GetFindingReportById(int id);
    }
}
