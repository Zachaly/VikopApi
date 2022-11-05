using MediatR;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Command;
using VikopApi.Application.Command.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Enums;
using VikopApi.Application.Reactions.Abstractions;

namespace VikopApi.Application.Reactions.Commands
{
    public class DeleteReactionCommand : IRequest<CommandResponseModel>
    {
        public int ObjectId { get; set; }
        private ReactionCommandType _type;
        public void SetComment() => _type = ReactionCommandType.DeleteComment;
        public void SetFinding() => _type = ReactionCommandType.DeleteFinding;
        public ReactionCommandType GetCommandType() => _type;
    }

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
