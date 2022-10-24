using MediatR;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.User.Abstractions;
using VikopApi.Mediator.Requests;

namespace VikopApi.Mediator.Handlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IAuthService _authService;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;

        public UpdateUserHandler(IAuthService authService, IUserService userService, IFileService fileService)
        {
            _authService = authService;
            _fileService = fileService;
            _userService = userService;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var updateRequest = new UpdateUserRequest
            {
                UserName = request.Username,
                Id = _authService.GetCurrentUserId(),
                Picture = ""
            };

            if (request.ProfilePicture != null)
            {
                _fileService.RemoveProfilePicture(updateRequest.Id);
                updateRequest.Picture = await _fileService.SaveProfilePicture(request.ProfilePicture);
            }

            await _userService.UpdateUser(updateRequest);

            return Unit.Value;
        }
    }
}
