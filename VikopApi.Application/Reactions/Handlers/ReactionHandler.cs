using MediatR;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Command.Abstractions;
using VikopApi.Application.Models.Command;
using VikopApi.Application.Models.Reaction.Commands;
using VikopApi.Application.Models.Reaction.Requests;
using VikopApi.Application.Reactions.Abstractions;

namespace VikopApi.Application.Reactions.Handlers
{
    public class ReactionHandler : IRequestHandler<ReactionCommand, CommandResponseModel>
    {
        private readonly IReactionService _reactionService;
        private readonly IAuthService _authService;
        private readonly ICommandResponseFactory _commandResponseFactory;

        public ReactionHandler(IReactionService reactionService, IAuthService authService, ICommandResponseFactory commandResponseFactory)
        {
            _reactionService = reactionService;
            _authService = authService;
            _commandResponseFactory = commandResponseFactory;
        }

        public async Task<CommandResponseModel> Handle(ReactionCommand request, CancellationToken cancellationToken)
        {
            var reaction = new AddReactionRequest
            {
                ObjectId = request.ObjectId,
                Reaction = request.Reaction,
                UserId = _authService.GetCurrentUserId(),
            };

            var res = false;
            var type = request.GetCommandType();
            if(type == ReactionCommandType.AddComment || type == ReactionCommandType.AddFinding)
            {
                if (type == ReactionCommandType.AddComment)
                {
                    res = await _reactionService.AddCommentReaction(reaction);
                }
                else if (type == ReactionCommandType.AddFinding)
                {
                    res = await _reactionService.AddFindingReaction(reaction);
                }
                if (!res)
                {
                    var errors = new Dictionary<string, IEnumerable<string>>();
                    errors.Add("Reaction", new string[] { "Reaction already exists" });

                    return _commandResponseFactory.CreateFailure(errors);
                }
            }

            if(type == ReactionCommandType.ChangeFinding || type == ReactionCommandType.ChangeComment)
            {
                if (request.GetCommandType() == ReactionCommandType.ChangeFinding)
                {
                    res = await _reactionService.ChangeFindingReaction(reaction);
                }
                else if (request.GetCommandType() == ReactionCommandType.ChangeComment)
                {
                    res = await _reactionService.ChangeCommentReaction(reaction);
                }

                if (!res)
                {
                    var errors = new Dictionary<string, IEnumerable<string>>();
                    errors.Add("Reaction", new string[] { "Reaction not found" });

                    return _commandResponseFactory.CreateFailure(errors);
                }
            }

            if (!res)
            {
                var errors = new Dictionary<string, IEnumerable<string>>();
                errors.Add("Reaction", new string[] { "Unknown command type" });

                return _commandResponseFactory.CreateFailure(errors);
            }
            
            return _commandResponseFactory.CreateSuccess();
        }
    }
}
