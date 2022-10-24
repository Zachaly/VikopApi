using MediatR;
using Microsoft.AspNetCore.Http;

namespace VikopApi.Mediator.Requests
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        public string Username { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
