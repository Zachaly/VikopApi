using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Models.Command;

namespace VikopApi.Application.Models.Post.Commands
{
    public class AddPostCommand : IRequest<DataCommandResponseModel<PostModel>>
    {
        public string Content { get; set; }
        public IFormFile? Picture { get; set; }
        public string? Tags { get; set; }
    }
}
