using MediatR;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Command.Abstractions;
using VikopApi.Application.Models.Command;
using VikopApi.Application.Models.Report.Commands;
using VikopApi.Application.Models.Report.Requests;
using VikopApi.Application.Reports.Abstractions;

namespace VikopApi.Application.Reports.Commands
{
    public class AddReportHandler : IRequestHandler<AddReportCommand, CommandResponseModel>
    {
        private readonly IReportService _reportService;
        private readonly IAuthService _authService;
        private readonly ICommandResponseFactory _commandReponseFactory;

        public AddReportHandler(IReportService reportService, IAuthService authService, ICommandResponseFactory commandResponseFactory)
        {
            _reportService = reportService;
            _authService = authService;
            _commandReponseFactory = commandResponseFactory;
        }

        public async Task<CommandResponseModel> Handle(AddReportCommand request, CancellationToken cancellationToken)
        {
            var addReportRequest = new AddReportRequest
            {
                ObjectId = request.ObjectId,
                Reason = request.Reason,
                ReportingUserId = _authService.GetCurrentUserId()
            };

            var result = false;
            try
            {
                if (request.GetCommandType() == ReportCommandType.Post)
                {
                    result = await _reportService.AddPostReport(addReportRequest);
                }
                else if (request.GetCommandType() == ReportCommandType.Finding)
                {
                    result = await _reportService.AddFindingReport(addReportRequest);
                }
            }
            catch (Exception exception)
            {
                var error = new Dictionary<string, IEnumerable<string>>();
                error.Add("Error", new string[] { exception.Message });
                return _commandReponseFactory.CreateFailure(error);
            }

            if (result)
            {
                return _commandReponseFactory.CreateSuccess();
            }

            var errors = new Dictionary<string, IEnumerable<string>>();
            errors.Add("DatabaseError", new string[] { "Exception occured while adding to database" });

            return _commandReponseFactory.CreateFailure(errors);
        }
    }
}
