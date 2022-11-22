using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Report.Commands;
using VikopApi.Application.Models.Report.Validators;
using VikopApi.Application.Reports.Abstractions;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IMediator _mediator;

        public ReportController(IReportService reportService, IMediator mediator)
        {
            _reportService = reportService;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("post")]
        [Authorize(Policy = "Moderator")]
        public IActionResult GetPostReports([FromQuery] PagedRequest request)
            => Ok(_reportService.GetPostReports(request));

        [HttpGet]
        [Route("finding")]
        [Authorize(Policy = "Moderator")]
        public IActionResult GetFindingReports([FromQuery] PagedRequest request)
            => Ok(_reportService.GetFindingReports(request));

        [HttpPost]
        [Route("post")]
        public async Task<IActionResult> AddPostReport(
            AddReportCommand addReportCommand,
            [FromServices] AddReportValidator validator)
        {
            var validation = validator.Validate(addReportCommand);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            addReportCommand.SetPost();

            return Ok(await _mediator.Send(addReportCommand));
        }

        [HttpPost]
        [Route("finding")]
        public async Task<IActionResult> AddFindingReport(
            AddReportCommand addReportCommand,
            [FromServices] AddReportValidator validator)
        {
            var validation = validator.Validate(addReportCommand);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            addReportCommand.SetFinding();

            return Ok(await _mediator.Send(addReportCommand));
        }

        [HttpDelete]
        [Route("finding")]
        [Authorize(Policy = "Moderator")]
        public async Task<IActionResult> RemoveFindingReport(RemoveReportCommand removeReportCommand)
        {
            removeReportCommand.SetFinding();

            return Ok(await _mediator.Send(removeReportCommand));
        }

        [HttpDelete]
        [Route("post")]
        [Authorize(Policy = "Moderator")]
        public async Task<IActionResult> RemovePostReport(RemoveReportCommand removeReportCommand)
        {
            removeReportCommand.SetPost();

            return Ok(await _mediator.Send(removeReportCommand));
        }

        [HttpGet("post/{id}")]
        [Authorize(Policy = "Moderator")]
        public IActionResult GetPostReportById(int id)
            => Ok(_reportService.GetPostReportById(id));

        [HttpGet("finding/{id}")]
        [Authorize(Policy = "Moderator")]
        public IActionResult GetFindingReportById(int id)
            => Ok(_reportService.GetFindingReportById(id));
    }
}
