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
        public IActionResult GetAll([FromServices] GetFindings getFindings)
            => Ok(getFindings.Execute());

        [HttpGet]
        public IActionResult New([FromServices] GetNewFindings getNewFindings)
            => Ok(getNewFindings.Execute());

        [HttpGet]
        public IActionResult Hot([FromServices] GetTopFindings getTopFindings)
            => Ok(getTopFindings.Execute());

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
    }
}
