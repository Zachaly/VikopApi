using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopApi.Domain.Enums;

namespace VikopApi.Mediator.Requests
{
    public class ChangeCommentReactionCommand : IRequest
    {
        public int CommentId { get; set; }
        public string UserId { get; set; }
        public Reaction Reaction { get; set; }
    }
}
