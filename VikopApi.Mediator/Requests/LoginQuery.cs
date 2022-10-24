using MediatR;
using VikopApi.Mediator.Responses;

namespace VikopApi.Mediator.Requests
{
    public class LoginQuery : IRequest<LoginResponse>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
