using VikopApi.Application.Models.Report;
using VikopApi.Application.Models.Report.Requests;

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
