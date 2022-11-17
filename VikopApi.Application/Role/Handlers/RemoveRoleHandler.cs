using MediatR;
using VikopApi.Application.Command.Abstractions;
using VikopApi.Application.Models.Command;
using VikopApi.Application.Models.Role.Commands;
using VikopApi.Application.Role.Abstractions;
using VikopApi.Application.User.Abstractions;
using VikopApi.Domain.Enums;

namespace VikopApi.Application.Role.Commands
{


    public class RemoveRoleHandler : IRequestHandler<RemoveRoleCommand, CommandResponseModel>
    {
        private readonly ICommandResponseFactory _commandResponseFactory;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public RemoveRoleHandler(ICommandResponseFactory commandResponseFactory, IRoleService roleService, IUserService userService)
        {
            _commandResponseFactory = commandResponseFactory;
            _roleService = roleService;
            _userService = userService;
        }

        public async Task<CommandResponseModel> Handle(RemoveRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _roleService.RemoveRole(request.UserId, request.Role);

            if (result.Succeeded)
            {
                if (request.Role == "Moderator")
                    await _userService.SetUserRank(request.UserId, Rank.Orange);
                else if (request.Role == "Admin")
                    await _userService.SetUserRank(request.UserId, Rank.Orange);

                return _commandResponseFactory.CreateSuccess();
            }

            var errors = new Dictionary<string, IEnumerable<string>>();
            errors.Add("Auth", result.Errors.Select(error => error.Description));

            return _commandResponseFactory.CreateFailure(errors);
        }
    }
}
