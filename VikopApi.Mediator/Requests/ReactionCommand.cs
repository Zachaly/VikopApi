using MediatR;
using VikopApi.Domain.Enums;
using VikopApi.Mediator.RequestEnums;

namespace VikopApi.Mediator.Requests
{
    public class ReactionCommand : IRequest<Unit>
    {
        public int ObjectId { get; set; }
        public Reaction Reaction { get; set; }
        public ReactionCommandType CommandType { get; private set; }

        public void SetCommandType(ReactionCommandType commandType) => CommandType = commandType;
    }
}
