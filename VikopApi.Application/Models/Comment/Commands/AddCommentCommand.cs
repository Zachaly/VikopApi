using Microsoft.AspNetCore.Http;

namespace VikopApi.Application.Models.Comment.Commands
{
    public abstract class AddCommentCommand
    {
        public string Content { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
