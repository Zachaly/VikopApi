using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Application.Findings.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Finding.Command;
using VikopApi.Application.Models.Finding.Requests;
using VikopApi.Application.Models.Finding.Validators;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]")]
    public class FindingController : ControllerBase
    {
        private readonly IFindingService _findingService;
        private readonly IMediator _mediator;

        public FindingController(IFindingService findingService, IMediator mediator)
        {
            _findingService = findingService;
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all findings
        /// </summary>
        /// <response code="200">
        /// List of findings containing:
        /// * id
        /// * title
        /// * description
        /// * creatorName
        /// * creatorId
        /// * link
        /// * commentCount
        /// * created - creation date
        /// </response>
        [HttpGet]
        public IActionResult Get([FromQuery] PagedRequest request)
            => Ok(_findingService.GetFindings(request));

        /// <summary>
        /// Gets finding with given id
        /// </summary>
        /// <response code="200">
        /// Finding model:
        /// * id
        /// * title
        /// * description
        /// * creatorName
        /// * creatorId
        /// * link
        /// * commentCount
        /// * created - creation date
        /// * comments: 
        ///     - content
        ///     - created
        ///     - id
        ///     - creatorName
        ///     - creatorId
        /// </response>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
            => Ok(_findingService.GetFindingById(id));

        /// <summary>
        /// Adds new finding
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(
            [FromForm] AddFindingCommand request,
            [FromServices] AddFindingValidator validator)
        {
            var validation = validator.Validate(request);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            await _mediator.Send(request);

            return Ok();
        }

        /// <summary>
        /// Gets count of all finding pages
        /// </summary>
        [HttpGet("count/{pageSize}")]
        public IActionResult PageCount(int pageSize)
            => Ok(_findingService.GetPageCount(pageSize));

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchFindingsRequest request)
            => Ok(_findingService.Search(request));
    }
}
