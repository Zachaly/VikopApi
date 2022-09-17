﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Api.DTO;
using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Application.Comments;
using VikopApi.Domain.Enums;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public CommentController(IAuthManager authManager)
        {
            _authManager = authManager;
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
            FindingCommentModel commentModel,
            [FromServices] AddFindingComment addComment)
        {
            var request = new AddFindingComment.Request
            {
                Content = commentModel.Content,
                CreatorId = _authManager.GetCurrentUserId(),
                FindingId = commentModel.FindingId
            };

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

        [HttpDelete("{commentId}")]
        public async Task<IActionResult> DeleteReaction(
            int commentId,
            [FromServices] DeleteReaction deleteReaction)
            => Ok(await deleteReaction.Execute(commentId, _authManager.GetCurrentUserId()));
    }
}