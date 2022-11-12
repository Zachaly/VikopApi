using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopApi.Application.Reports.Abstractions;
using VikopApi.Application.Reports.Commands;

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
        public IActionResult GetPostReports([FromQuery] int? pageIndex, [FromQuery] int? pageSize)
            => Ok(_reportService.GetPostReports(pageIndex, pageSize));

        [HttpGet]
        [Route("finding")]
        [Authorize(Policy = "Moderator")]
        public IActionResult GetFindingReports([FromQuery] int? pageIndex, [FromQuery] int? pageSize)
            => Ok(_reportService.GetFindingReports(pageIndex, pageSize));

        [HttpPost]
        [Route("post")]
        public async Task<IActionResult> AddPostReport(AddReportCommand addReportCommand)
        {
            addReportCommand.SetPost();

            return Ok(await _mediator.Send(addReportCommand));
        }

        [HttpPost]
        [Route("finding")]
        public async Task<IActionResult> AddFindingReport(AddReportCommand addReportCommand)
        {
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
