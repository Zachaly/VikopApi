using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Models;

namespace VikopApi.Mediator.Requests
{
    public class AddSubcommentQuery : IRequest<CommentModel>
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
