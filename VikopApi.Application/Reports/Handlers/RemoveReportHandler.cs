using MediatR;
using VikopApi.Application.Command.Abstractions;
using VikopApi.Application.Findings.Abstractions;
using VikopApi.Application.Models.Command;
using VikopApi.Application.Models.Report.Commands;
using VikopApi.Application.Posts.Abstractions;
using VikopApi.Application.Reports.Abstractions;

namespace VikopApi.Application.Reports.Commands
{
    public class RemoveReportHandler : IRequestHandler<RemoveReportCommand, CommandResponseModel>
    {
        private readonly IReportService _reportService;
        private readonly IFindingService _findingService;
        private readonly IPostService _postService;
        private readonly ICommandResponseFactory _commandResponseFactory;

        public RemoveReportHandler(IReportService reportService, IFindingService findingService,
            IPostService postService, ICommandResponseFactory commandResponseFactory)
        {
            _reportService = reportService;
            _findingService = findingService;
            _postService = postService;
            _commandResponseFactory = commandResponseFactory;
        }

        public async Task<CommandResponseModel> Handle(RemoveReportCommand request, CancellationToken cancellationToken)
        {
            if (request.GetCommandType() == ReportCommandType.Finding)
            {
                await RemoveFinding(request.ReportId, request.Accepted);
            }
            else if (request.GetCommandType() == ReportCommandType.Post)
            {
                await RemovePost(request.ReportId, request.Accepted);
            }

            return _commandResponseFactory.CreateSuccess();
        }

        public async Task RemoveFinding(int id, bool accepted)
        {
            var findingId = _reportService.GetFindingReportById(id).Finding.Id;

            await _reportService.RemoveFindingReport(findingId);

            if (accepted)
                await _findingService.RemoveFindingById(findingId);
        }

        public async Task RemovePost(int id, bool accepted)
        {
            var postId = _reportService.GetPostReportById(id).Post.Id;

            await _reportService.RemovePostReport(postId);

            if (accepted)
                await _postService.RemovePostById(postId);
        }
    }
}
