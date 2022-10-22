using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Api.DTO;
using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Api.Infrastructure.FileManager;
using VikopApi.Application.Comments;
using VikopApi.Application.Comments.Abstractions;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Reactions.Abstractions;
using VikopApi.Domain.Enums;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly IFileManager _fileManager;
        private readonly ICommentService _commentService;
        private readonly IReactionService _reactionService;

        public CommentController(IAuthManager authManager, IFileManager fileManager,
            ICommentService commentService, IReactionService reactionService)
        {
            _authManager = authManager;
            _fileManager = fileManager;
            _commentService = commentService;
            _reactionService = reactionService;
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
            [FromForm] FindingCommentModel commentModel)
        {
            var request = new AddFindingCommentRequest
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

            return Ok(await _commentService.AddFindingComment(request));
        }

        /// <summary>
        /// Adds reaction to comment
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> AddReaction(
            ReactionModel reactionModel)
        {
            var request = new AddReactionRequest
            {
                ObjectId = reactionModel.Id,
                Reaction = (Reaction)reactionModel.Reaction,
                UserId = _authManager.GetCurrentUserId(),
            };

            await _reactionService.AddCommentReaction(request);

            return Ok();
        }

        /// <summary>
        /// Changes reaction of current user to given comment
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> ChangeReaction(
            ReactionModel reactionModel)
        {
            var request = new AddReactionRequest
            {
                ObjectId = reactionModel.Id,
                Reaction = (Reaction)reactionModel.Reaction,
                UserId = _authManager.GetCurrentUserId(),
            };

            await _reactionService.ChangeCommentReaction(request);

            return Ok();
        }

        /// <summary>
        /// Deletes current user's reaction
        /// </summary>
        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteReaction(int commentId)
        {
            await _reactionService.DeleteCommentReaction(commentId, _authManager.GetCurrentUserId());

            return Ok();
        }
            

        /// <summary>
        /// Get current user's reaction of given comment
        /// </summary>
        [HttpGet("{commentId}")]
        public IActionResult CurrentUserReaction(int commentId)
            => Ok(_reactionService.GetCommentReaction(commentId, _authManager.GetCurrentUserId()));


        [HttpGet("{commentId}")]
        [AllowAnonymous]
        public IActionResult SubComments(int commentId)
            => Ok(_commentService.GetSubcomments(commentId));

        [HttpPost]
        public async Task<IActionResult> AddSubComment([FromForm] SubCommentModel subComment)
        {
            var request = new AddSubcommentRequest
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

            return Ok(await _commentService.AddSubcomment(request));
        }
    }
}
