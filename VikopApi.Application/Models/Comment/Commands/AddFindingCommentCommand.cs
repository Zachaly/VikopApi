using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Models.Command;

namespace VikopApi.Application.Models.Comment.Commands
{
    public class AddFindingCommentCommand : IRequest<DataCommandResponseModel<CommentModel>>
    {
        public int FindingId { get; set; }
        public string Content { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
