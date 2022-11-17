using VikopApi.Application.Models.Finding;
using VikopApi.Application.Models.Post;
using VikopApi.Application.Models.User;

namespace VikopApi.Application.Models.Report
{
    public abstract class ReportModel
    {
        public int Id { get; set; }
        public UserListItemModel ReportingUser { get; set; }
        public string Reason { get; set; }
        public string Created { get; set; }
    }

    public class PostReportModel : ReportModel
    {
        public PostModel Post { get; set; }
    }

    public class FindingReportModel : ReportModel
    {
        public FindingListItemModel Finding { get; set; }
    }
}
