using MediatR;
using VikopApi.Application.Models.Command;

namespace VikopApi.Application.Models.Comment.Commands
{
    public class AddSubcommentCommand : AddCommentCommand, IRequest<DataCommandResponseModel<CommentModel>>
    {
        public int CommentId { get; set; }
    }
}
