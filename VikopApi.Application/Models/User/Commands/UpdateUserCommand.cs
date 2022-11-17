using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Models.Command;

namespace VikopApi.Application.Models.User.Commands
{
    public class UpdateUserCommand : IRequest<CommandResponseModel>
    {
        public string Username { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
