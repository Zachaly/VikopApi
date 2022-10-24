using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Models;

namespace VikopApi.Mediator.Requests
{
    public class AddFindingCommentQuery : IRequest<CommentModel>
    {
        public int FindingId { get; set; }
        public string Content { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
