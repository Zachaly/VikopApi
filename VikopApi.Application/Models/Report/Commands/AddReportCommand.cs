using MediatR;
using VikopApi.Application.Models.Command;

namespace VikopApi.Application.Models.Report.Commands
{
    public class AddReportCommand : IRequest<CommandResponseModel>
    {
        public int ObjectId { get; set; }
        public string Reason { get; set; }
        private ReportCommandType _commandType;
        public void SetFinding() => _commandType = ReportCommandType.Finding;
        public void SetPost() => _commandType = ReportCommandType.Post;
        public ReportCommandType GetCommandType() => _commandType;
    }
}
