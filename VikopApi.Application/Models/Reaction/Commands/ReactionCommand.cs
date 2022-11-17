using MediatR;
using VikopApi.Application.Models.Command;

namespace VikopApi.Application.Models.Reaction.Commands
{
    public class ReactionCommand : IRequest<CommandResponseModel>
    {
        public int ObjectId { get; set; }
        public Domain.Enums.Reaction Reaction { get; set; }
        ReactionCommandType _type;

        public void SetCommandType(ReactionCommandType commandType) => _type = commandType;
        public ReactionCommandType GetCommandType() => _type;
    }
}
