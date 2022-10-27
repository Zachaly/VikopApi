using MediatR;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Models.Enums;
using VikopApi.Application.Reactions.Abstractions;

namespace VikopApi.Application.Reactions.Commands
{
    public class DeleteReactionCommand : IRequest
    {
        public int ObjectId { get; set; }
        private ReactionCommandType _type;
        public void SetComment() => _type = ReactionCommandType.DeleteComment;
        public void SetFinding() => _type = ReactionCommandType.DeleteFinding;
        public ReactionCommandType GetCommandType() => _type;
    }

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

            if (request.GetCommandType() == ReactionCommandType.DeleteFinding)
            {
                await _reactionService.DeleteFindingReaction(request.ObjectId, userId);
            }
            else if (request.GetCommandType() == ReactionCommandType.DeleteComment)
            {
                await _reactionService.DeleteCommentReaction(request.ObjectId, userId);
            }

            return Unit.Value;
        }
    }
}
