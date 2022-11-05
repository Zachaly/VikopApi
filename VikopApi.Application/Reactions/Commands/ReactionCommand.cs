using MediatR;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Models.Enums;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Reactions.Abstractions;
using VikopApi.Domain.Enums;

namespace VikopApi.Application.Reactions.Commands
{
    public class ReactionCommand : IRequest<Unit>
    {
        public int ObjectId { get; set; }
        public Reaction Reaction { get; set; }
        ReactionCommandType _type;

        public void SetCommandType(ReactionCommandType commandType) => _type = commandType;
        public ReactionCommandType GetCommandType() => _type;
    }

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

            var res = false;

            if (request.GetCommandType() == ReactionCommandType.AddComment)
            {
                await _reactionService.AddCommentReaction(reaction);
            }
            else if (request.GetCommandType() == ReactionCommandType.AddFinding)
            {
                await _reactionService.AddFindingReaction(reaction);
            }
            else if (request.GetCommandType() == ReactionCommandType.ChangeFinding)
            {
                await _reactionService.ChangeFindingReaction(reaction);
            }
            else if (request.GetCommandType() == ReactionCommandType.ChangeComment)
            {
                await _reactionService.ChangeCommentReaction(reaction);
            }

            return Unit.Value;
        }
    }
}
