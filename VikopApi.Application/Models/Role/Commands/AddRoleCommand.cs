using MediatR;
using VikopApi.Application.Models.Command;

namespace VikopApi.Application.Models.Role.Commands
{
    public class AddRoleCommand : IRequest<CommandResponseModel>
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}
