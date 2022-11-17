using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Models.Command;

namespace VikopApi.Application.Models.Comment.Commands
{
    public class AddSubcommentCommand : IRequest<DataCommandResponseModel<CommentModel>>
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
