using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Api.DTO;
using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Api.Infrastructure.FileManager;
using VikopApi.Application.Findings;
using VikopApi.Domain.Enums;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class FindingController : ControllerBase
    {
        private readonly int _pageSize;

        public FindingController(IConfiguration config)
        {
            _pageSize = int.Parse(config["PageSize"]);
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
        public IActionResult All([FromServices] GetFindings getFindings, int? pageIndex = 0)
            => Ok(getFindings.Execute(pageIndex, _pageSize));

        [HttpGet("{pageIndex?}")]
        public IActionResult New([FromServices] GetNewFindings getNewFindings, int? pageIndex = 0)
            => Ok(getNewFindings.Execute(pageIndex, _pageSize));

        [HttpGet("{pageIndex?}")]
        public IActionResult Hot([FromServices] GetTopFindings getTopFindings, int? pageIndex = 0)
            => Ok(getTopFindings.Execute(pageIndex, _pageSize));

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
        public IActionResult Get(int id, [FromServices] GetFinding getFinding)
            => Ok(getFinding.Execute(id));

        /// <summary>
        /// Adds new finding
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(
            [FromForm] AddFindingModel request,
            [FromServices] AddFinding addFinding,
            [FromServices] IAuthManager authManager,
            [FromServices] IFileManager fileManager) 
            => Ok(await addFinding.Execute(new AddFinding.Request
                {
                    Title = request.Title,
                    CreatorId = authManager.GetCurrentUserId(),
                    Link = request.Link,
                    Description = request.Description,
                    Picture = await fileManager.SaveFindingPicture(request.Picture),
                }));

        /// <summary>
        /// Adds reaction to finding
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddReaction(
            ReactionModel reactionModel,
            [FromServices] AddReaction addReaction,
            [FromServices] IAuthManager authManager)
        {
            var request = new AddReaction.Request
            {
                FindingId = reactionModel.Id,
                Reaction = (Reaction)reactionModel.Reaction,
                UserId = authManager.GetCurrentUserId()
            };

            return Ok(await addReaction.Execute(request));
        }

        /// <summary>
        /// Changes reaction of current user 
        /// </summary>
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangeReaction(
            ReactionModel reactionModel,
            [FromServices] ChangeReaction addReaction,
            [FromServices] IAuthManager authManager)
        {
            var request = new ChangeReaction.Request
            {
                FindingId = reactionModel.Id,
                Reaction = (Reaction)reactionModel.Reaction,
                UserId = authManager.GetCurrentUserId()
            };

            return Ok(await addReaction.Execute(request));
        }

        /// <summary>
        /// Removes reaction of current user
        /// </summary>
        [HttpDelete("{findingId}")]
        [Authorize]
        public async Task<IActionResult> DeleteReaction(
            int findingId,
            [FromServices] DeleteReaction deleteReaction,
            [FromServices] IAuthManager authManager)
            => Ok(await deleteReaction.Execute(findingId, authManager.GetCurrentUserId()));

        /// <summary>
        /// Get current user's reaction of given finding
        /// </summary>
        [HttpGet("{findingId}")]
        [Authorize]
        public IActionResult CurrentUserReaction(
            int findingId,
            [FromServices] IAuthManager authManager,
            [FromServices] GetUserReaction getUserReaction)
            => Ok(getUserReaction.Execute(findingId, authManager.GetCurrentUserId()));

        /// <summary>
        /// Gets count of all finding pages
        /// </summary>
        [HttpGet]
        public IActionResult PageCount([FromServices] GetPageCount getPageCount)
            => Ok(getPageCount.Execute(_pageSize));
    }
}
