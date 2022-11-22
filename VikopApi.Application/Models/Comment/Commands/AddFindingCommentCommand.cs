using MediatR;
using VikopApi.Application.Models.Command;

namespace VikopApi.Application.Models.Comment.Commands
{
    public class AddFindingCommentCommand : AddCommentCommand, IRequest<DataCommandResponseModel<CommentModel>>
    {
        public int FindingId { get; set; }
    }
}
