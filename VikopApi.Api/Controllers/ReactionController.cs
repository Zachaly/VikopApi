using MediatR;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Application.Models.Enums;
using VikopApi.Application.Reactions.Abstractions;
using VikopApi.Application.Reactions.Commands;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]")]
    public class ReactionController : ControllerBase
    {
        private readonly IReactionService _reactionService;
        private readonly IMediator _mediator;

        public ReactionController(IReactionService reactionService, IMediator mediator)
        {
            _reactionService = reactionService;
            _mediator = mediator;
        }

        /// <summary>
        /// Adds reaction to finding
        /// </summary>
        [HttpPost]
        [Route("finding")]
        public async Task<IActionResult> AddFindingReaction(ReactionCommand request)
        {
            request.SetCommandType(ReactionCommandType.AddFinding);

            await _mediator.Send(request);

            return Ok();
        }

        /// <summary>
        /// Adds reaction to comment
        /// </summary>
        [HttpPost]
        [Route("comment")]
        public async Task<IActionResult> AddCommentReaction(ReactionCommand request)
        {
            request.SetCommandType(ReactionCommandType.AddComment);

            await _mediator.Send(request);

            return Ok();
        }

        [HttpPut]
        [Route("finding")]
        public async Task<IActionResult> ChangeFindingReaction(ReactionCommand request)
        {
            request.SetCommandType(ReactionCommandType.ChangeFinding);

            await _mediator.Send(request);

            return Ok();
        }

        [HttpPut]
        [Route("comment")]
        public async Task<IActionResult> ChangeCommentReaction(ReactionCommand request)
        {
            request.SetCommandType(ReactionCommandType.ChangeComment);

            await _mediator.Send(request);

            return Ok();
        }

        [HttpGet("finding/{findingId}/{userId}")]
        public IActionResult GetFindingReaction(int findingId, string userId)
            => Ok(_reactionService.GetFindingReaction(findingId, userId));

        [HttpGet("comment/{commentId}/{userId}")]
        public IActionResult GetCommentReaction(int commentId, string userId)
            => Ok(_reactionService.GetCommentReaction(commentId, userId));

        [HttpDelete]
        [Route("comment")]
        public async Task<IActionResult> DeleteCommentReaction(DeleteReactionCommand command)
        {
            command.SetComment();

            await _mediator.Send(command);

            return Ok();
        }

        [HttpDelete]
        [Route("finding")]
        public async Task<IActionResult> DeleteFindingReaction(DeleteReactionCommand command)
        {
            command.SetFinding();

            await _mediator.Send(command);

            return Ok();
        }
    }
}
