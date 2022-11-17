using MediatR;
using VikopApi.Application.Models.Command;

namespace VikopApi.Application.Models.Report.Commands
{
    public class RemoveReportCommand : IRequest<CommandResponseModel>
    {
        public int ReportId { get; set; }
        public bool Accepted { get; set; }
        private ReportCommandType _commandType;
        public void SetFinding() => _commandType = ReportCommandType.Finding;
        public void SetPost() => _commandType = ReportCommandType.Post;
        public ReportCommandType GetCommandType() => _commandType;
    }
}
