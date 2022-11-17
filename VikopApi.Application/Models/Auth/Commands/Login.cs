using MediatR;
using VikopApi.Application.Models.Command;

namespace VikopApi.Application.Models.Auth.Commands
{
    public class LoginCommand : IRequest<DataCommandResponseModel<LoginResponse>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; } = "";
        public IEnumerable<string> Claims { get; set; }
    }
}
