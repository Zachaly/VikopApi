using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Api.DTO;
using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Application.Comments;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CommentController : ControllerBase
    {
        /// <summary>
        /// Adds comment to finding
        /// </summary>
        /// <response code="200">
        /// Comment model:
        /// * id
        /// * creatorId
        /// * creatorName
        /// * content
        /// * created - creation date
        /// </response>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddFindingComment(
            FindingCommentModel commentModel,
            [FromServices] IAuthManager authManager,
            [FromServices] AddFindingComment addComment)
        {
            var request = new AddFindingComment.Request
            {
                Content = commentModel.Content,
                CreatorId = authManager.GetCurrentUserId(),
                FindingId = commentModel.FindingId
            };

            return Ok(await addComment.Execute(request));
        }
    }
}
