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

            return Ok(await _mediator.Send(request));
        }

        /// <summary>
        /// Adds reaction to comment
        /// </summary>
        [HttpPost]
        [Route("comment")]
        public async Task<IActionResult> AddCommentReaction(ReactionCommand request)
        {
            request.SetCommandType(ReactionCommandType.AddComment);

            return Ok(await _mediator.Send(request));
        }

        [HttpPut]
        [Route("finding")]
        public async Task<IActionResult> ChangeFindingReaction(ReactionCommand request)
        {
            request.SetCommandType(ReactionCommandType.ChangeFinding);

            return Ok(await _mediator.Send(request));
        }

        [HttpPut]
        [Route("comment")]
        public async Task<IActionResult> ChangeCommentReaction(ReactionCommand request)
        {
            request.SetCommandType(ReactionCommandType.ChangeComment);

            return Ok(await _mediator.Send(request));
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

            return Ok(await _mediator.Send(command));
        }

        [HttpDelete]
        [Route("finding")]
        public async Task<IActionResult> DeleteFindingReaction([FromBody] DeleteReactionCommand command)
        {
            command.SetFinding();

            return Ok(await _mediator.Send(command));
        }
    }
}
