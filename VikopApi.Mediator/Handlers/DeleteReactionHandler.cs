using MediatR;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Reactions.Abstractions;
using VikopApi.Mediator.RequestEnums;
using VikopApi.Mediator.Requests;

namespace VikopApi.Mediator.Handlers
{
    public class DeleteReactionHandler : IRequestHandler<DeleteReactionCommand>
    {
        private readonly IReactionService _reactionService;
        private readonly IAuthService _authService;

        public DeleteReactionHandler(IReactionService reactionService, IAuthService authService)
        {
            _reactionService = reactionService;
            _authService = authService;
        }

        public async Task<Unit> Handle(DeleteReactionCommand request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetCurrentUserId();

            if (request.ReactionCommandType == ReactionCommandType.DeleteFinding)
            {
                await _reactionService.DeleteFindingReaction(request.ObjectId, userId);
            }
            else if(request.ReactionCommandType == ReactionCommandType.DeleteComment)
            {
                await _reactionService.DeleteCommentReaction(request.ObjectId, userId);
            }

            return Unit.Value;
        }
    }
}
