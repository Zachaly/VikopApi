using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Application.Findings.Abstractions;
using VikopApi.Application.Findings.Commands;
using VikopApi.Application.Models.Finding.Command;
using VikopApi.Application.Models.Finding.Requests;
using VikopApi.Domain.Enums;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class FindingController : ControllerBase
    {
        private readonly int _pageSize;
        private readonly IFindingService _findingService;
        private readonly IMediator _mediator;

        public FindingController(IConfiguration config,
            IFindingService findingService,
            IMediator mediator)
        {
            _pageSize = int.Parse(config["PageSize"]);
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
        [HttpGet("{pageIndex:int?}")]
        public IActionResult All(int? pageIndex = 0)
            => Ok(_findingService.GetFindings(null, pageIndex, _pageSize));

        [HttpGet("{pageIndex?}")]
        public IActionResult New(int? pageIndex = 0)
            => Ok(_findingService.GetFindings(SortingType.New, pageIndex, _pageSize));

        [HttpGet("{pageIndex?}")]
        public IActionResult Hot(int? pageIndex = 0)
            => Ok(_findingService.GetFindings(SortingType.Top, pageIndex, _pageSize));

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
        public async Task<IActionResult> Add([FromForm] AddFindingCommand request)
        {
            await _mediator.Send(request);

           return Ok();
        }

        /// <summary>
        /// Gets count of all finding pages
        /// </summary>
        [HttpGet]
        public IActionResult PageCount()
            => Ok(_findingService.GetPageCount(_pageSize));

        [HttpGet]
        public IActionResult Search([FromQuery] SearchFindingsRequest request)
            => Ok(_findingService.Search(request));
    }
}
