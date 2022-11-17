using MediatR;
using VikopApi.Application.Models.Command;

namespace VikopApi.Application.Models.Reaction.Commands
{
    public class DeleteReactionCommand : IRequest<CommandResponseModel>
    {
        public int ObjectId { get; set; }
        private ReactionCommandType _type;
        public void SetComment() => _type = ReactionCommandType.DeleteComment;
        public void SetFinding() => _type = ReactionCommandType.DeleteFinding;
        public ReactionCommandType GetCommandType() => _type;
    }
}
