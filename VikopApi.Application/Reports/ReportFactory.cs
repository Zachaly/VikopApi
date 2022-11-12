using VikopApi.Application.Findings.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Posts.Abstractions;
using VikopApi.Application.Reports.Abstractions;
using VikopApi.Application.User.Abstractions;

namespace VikopApi.Application.Reports
{
    [Implementation(typeof(IReportFactory))]
    public class ReportFactory : IReportFactory
    {
        private readonly IFindingFactory _findingFactory;
        private readonly IPostFactory _postFactory;
        private readonly IUserFactory _userFactory;

        public ReportFactory(IFindingFactory findingFactory, IPostFactory postFactory, IUserFactory userFactory)
        {
            _findingFactory = findingFactory;
            _postFactory = postFactory;
            _userFactory = userFactory;
        }

        public FindingReport CreateFinding(AddReportRequest addReportRequest)
            => new FindingReport
            {
                FindingId = addReportRequest.ObjectId,
                Reason = addReportRequest.Reason,
                Created = DateTime.Now,
                ReportingUserId = addReportRequest.ReportingUserId
            };

        public ReportListItem CreateListItem(FindingReport report)
            => new ReportListItem
            {
                Id = report.Id,
                Created = report.Created.GetTime(),
                ObjectId = report.FindingId.GetValueOrDefault(),
                ReportingUser = _userFactory.CreateListItem(report.ReportingUser),
            };

        public ReportListItem CreateListItem(PostReport report)
            => new ReportListItem
            {
                Id = report.Id,
                Created = report.Created.GetTime(),
                ObjectId = report.PostId.GetValueOrDefault(),
                ReportingUser = _userFactory.CreateListItem(report.ReportingUser)
            };

        public FindingReportModel CreateModel(FindingReport report)
            => new FindingReportModel
            {
                Id = report.Id,
                Finding = _findingFactory.CreateListItem(report.Finding),
                Reason = report.Reason ?? "",
                ReportingUser = _userFactory.CreateListItem(report.ReportingUser),
                Created = report.Created.GetTime(),
            };

        public PostReportModel CreateModel(PostReport report)
            => new PostReportModel
            {
                Id = report.Id,
                Post = _postFactory.CreateModel(report.Post),
                Reason = report.Reason ?? "",
                ReportingUser = _userFactory.CreateListItem(report.ReportingUser),
                Created = report.Created.GetTime(),
            };

        public PostReport CreatePost(AddReportRequest addReportRequest)
            => new PostReport
            {
                PostId = addReportRequest.ObjectId,
                Reason = addReportRequest.Reason,
                Created = DateTime.Now,
                ReportingUserId = addReportRequest.ReportingUserId
            };
    }
}
