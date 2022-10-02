using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Api.DTO;
using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Api.Infrastructure.FileManager;
using VikopApi.Application.Comments;
using VikopApi.Domain.Enums;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly IFileManager _fileManager;

        public CommentController(IAuthManager authManager, IFileManager fileManager)
        {
            _authManager = authManager;
            _fileManager = fileManager;
        }

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
        public async Task<IActionResult> AddFindingComment(
            [FromForm] FindingCommentModel commentModel,
            [FromServices] AddFindingComment addComment)
        {
            var request = new AddFindingComment.Request
            {
                Content = commentModel.Content,
                CreatorId = _authManager.GetCurrentUserId(),
                FindingId = commentModel.FindingId,
                Picture = ""
            };

            if(commentModel.Picture != null)
            {
                request.Picture = await _fileManager.SaveCommentPicture(commentModel.Picture);
            }

            return Ok(await addComment.Execute(request));
        }

        /// <summary>
        /// Adds reaction to comment
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddReaction(
            ReactionModel reactionModel,
            [FromServices] AddReaction addReaction)
        {
            var request = new AddReaction.Request
            {
                CommentId = reactionModel.Id,
                Reaction = (Reaction)reactionModel.Reaction,
                UserId = _authManager.GetCurrentUserId(),
            };

            return Ok(await addReaction.Execute(request));
        }

        /// <summary>
        /// Changes reaction of current user to given comment
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> ChangeReaction(
            ReactionModel reactionModel,
            [FromServices] ChangeReaction changeReaction)
        {
            var request = new ChangeReaction.Request
            {
                CommentId = reactionModel.Id,
                Reaction = (Reaction)reactionModel.Reaction,
                UserId = _authManager.GetCurrentUserId(),
            };

            return Ok(await changeReaction.Execute(request));
        }

        /// <summary>
        /// Deletes current user's reaction
        /// </summary>
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteReaction(
            int commentId,
            [FromServices] DeleteReaction deleteReaction)
            => Ok(await deleteReaction.Execute(commentId, _authManager.GetCurrentUserId()));

        /// <summary>
        /// Get current user's reaction of given comment
        /// </summary>
        [HttpGet("{commentId}")]
        public IActionResult CurrentUserReaction(
            int commentId,
            [FromServices] GetUserReaction getUserReaction)
            => Ok(getUserReaction.Execute(commentId, _authManager.GetCurrentUserId()));


        [HttpGet("{commentId}")]
        [AllowAnonymous]
        public IActionResult SubComments(
            int commentId,
            [FromServices] GetSubComments getSubComments)
            => Ok(getSubComments.Execute(commentId));

        [HttpPost]
        public async Task<IActionResult> AddSubComment(
            [FromForm] SubCommentModel subComment,
            [FromServices] AddSubComment addSubComment)
        {
            var request = new AddSubComment.Request
            {
                Content = subComment.Content,
                CreatorId = _authManager.GetCurrentUserId(),
                MainCommentId = subComment.CommentId,
                Picture = ""
            };

            if (subComment.Picture != null)
            {
                request.Picture = await _fileManager.SaveCommentPicture(subComment.Picture);
            }

            return Ok(await addSubComment.Execute(request));
        }
    }
}
