using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.Reports.Abstractions
{
    public interface IReportFactory
    {
        PostReport CreatePost(AddReportRequest addReportRequest);
        FindingReport CreateFinding(AddReportRequest addReportRequest);
        FindingReportModel CreateModel(FindingReport report);
        PostReportModel CreateModel(PostReport report);
        ReportListItem CreateListItem(FindingReport report);
        ReportListItem CreateListItem(PostReport report);
    }
}
