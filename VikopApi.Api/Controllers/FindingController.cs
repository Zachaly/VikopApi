using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Api.DTO;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Findings.Abstractions;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Reactions.Abstractions;
using VikopApi.Domain.Enums;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class FindingController : ControllerBase
    {
        private readonly int _pageSize;
        private readonly IFindingService _findingService;
        private readonly IAuthService _authService;
        private readonly IFileService _fileService;
        private readonly IReactionService _reactionService;

        public FindingController(IConfiguration config,
            IFindingService findingService,
            IAuthService authManager,
            IFileService fileService,
            IReactionService reactionService)
        {
            _pageSize = int.Parse(config["PageSize"]);
            _findingService = findingService;
            _authService = authManager;
            _fileService = fileService;
            _reactionService = reactionService;
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
        public async Task<IActionResult> Add([FromForm] AddFindingModel request)
        {
            await _findingService.AddFinding(new AddFindingRequest
            {
                Title = request.Title,
                CreatorId = _authService.GetCurrentUserId(),
                Link = request.Link,
                Description = request.Description,
                Picture = await _fileService.SaveFindingPicture(request.Picture),
                TagList = request.Tags.Split(',')
            });

           return Ok();
        }

        /// <summary>
        /// Adds reaction to finding
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReaction(ReactionModel reactionModel)
        {
            var request = new AddReactionRequest
            {
                ObjectId = reactionModel.Id,
                Reaction = (Reaction)reactionModel.Reaction,
                UserId = _authService.GetCurrentUserId()
            };

            await _reactionService.AddFindingReaction(request);

            return Ok();
        }

        /// <summary>
        /// Changes reaction of current user 
        /// </summary>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangeReaction(ReactionModel reactionModel)
        {
            var request = new AddReactionRequest
            {
                ObjectId = reactionModel.Id,
                Reaction = (Reaction)reactionModel.Reaction,
                UserId = _authService.GetCurrentUserId()
            };

            await _reactionService.ChangeFindingReaction(request);

            return Ok();
        }

        /// <summary>
        /// Removes reaction of current user
        /// </summary>
        [HttpDelete("{findingId}")]
        [Authorize]
        public async Task<IActionResult> DeleteReaction(int findingId)
        {
            await _reactionService.DeleteFindingReaction(findingId, _authService.GetCurrentUserId());

            return Ok();
        }
            

        /// <summary>
        /// Get current user's reaction of given finding
        /// </summary>
        [HttpGet("{findingId}")]
        [Authorize]
        public IActionResult CurrentUserReaction(int findingId)
            => Ok(_reactionService.GetFindingReaction(findingId, _authService.GetCurrentUserId()));

        /// <summary>
        /// Gets count of all finding pages
        /// </summary>
        [HttpGet]
        public IActionResult PageCount()
            => Ok(_findingService.GetPageCount(_pageSize));
    }
}
