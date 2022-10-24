using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Models;

namespace VikopApi.Mediator.Requests
{
    public class AddPostQuery : IRequest<PostModel>
    {
        public string Content { get; set; }
        public IFormFile? Picture { get; set; }
        public string Tags { get; set; }
    }
}
