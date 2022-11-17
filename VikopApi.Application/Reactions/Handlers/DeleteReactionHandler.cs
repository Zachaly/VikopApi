using MediatR;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Command.Abstractions;
using VikopApi.Application.Models.Command;
using VikopApi.Application.Models.Reaction.Commands;
using VikopApi.Application.Reactions.Abstractions;

namespace VikopApi.Application.Reactions.Handlers
{
    public class DeleteReactionHandler : IRequestHandler<DeleteReactionCommand, CommandResponseModel>
    {
        private readonly IReactionService _reactionService;
        private readonly IAuthService _authService;
        private readonly ICommandResponseFactory _commandResponseFactory;

        public DeleteReactionHandler(IReactionService reactionService, IAuthService authService, ICommandResponseFactory commandResponseFactory)
        {
            _reactionService = reactionService;
            _authService = authService;
            _commandResponseFactory = commandResponseFactory;
        }

        public async Task<CommandResponseModel> Handle(DeleteReactionCommand request, CancellationToken cancellationToken)
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

            return _commandResponseFactory.CreateSuccess();
        }
    }
}
