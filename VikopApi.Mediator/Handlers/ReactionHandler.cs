using MediatR;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Reactions.Abstractions;
using VikopApi.Mediator.RequestEnums;
using VikopApi.Mediator.Requests;

namespace VikopApi.Mediator.Handlers
{
    public class ReactionHandler : IRequestHandler<ReactionCommand>
    {
        private readonly IReactionService _reactionService;
        private readonly IAuthService _authService;

        public ReactionHandler(IReactionService reactionService, IAuthService authService)
        {
            _reactionService = reactionService;
            _authService = authService;
        }

        public async Task<Unit> Handle(ReactionCommand request, CancellationToken cancellationToken)
        {
            var reaction = new AddReactionRequest
            {
                ObjectId = request.ObjectId,
                Reaction = request.Reaction,
                UserId = _authService.GetCurrentUserId(),
            };

            if(request.CommandType == ReactionCommandType.AddComment)
            {
                await _reactionService.AddCommentReaction(reaction);
            }
            else if(request.CommandType == ReactionCommandType.AddFinding)
            {
                await _reactionService.AddFindingReaction(reaction);
            }
            else if(request.CommandType == ReactionCommandType.ChangeFinding)
            {
                await _reactionService.ChangeFindingReaction(reaction);
            }
            else if(request.CommandType == ReactionCommandType.ChangeComment)
            {
                await _reactionService.ChangeCommentReaction(reaction);
            }

            return Unit.Value;
        }
    }
}
