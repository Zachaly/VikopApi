using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopApi.Mediator.RequestEnums;

namespace VikopApi.Mediator.Requests
{
    public class DeleteReactionCommand : IRequest
    {
        public int ObjectId { get; set; }
        public ReactionCommandType ReactionCommandType { get; private set; }
        public void SetComment() => ReactionCommandType = ReactionCommandType.DeleteComment;
        public void SetFinding() => ReactionCommandType = ReactionCommandType.DeleteFinding;
    }
}
