using MediatR;
using VikopApi.Application.Command.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Role.Abstractions;
using VikopApi.Application.User.Abstractions;
using VikopApi.Domain.Enums;

namespace VikopApi.Application.Role.Commands
{
    public class AddRoleCommand : IRequest<CommandResponseModel>
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }

    public class AddRoleHandler : IRequestHandler<AddRoleCommand, CommandResponseModel>
    {
        private readonly ICommandResponseFactory _commandResponseFactory;
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;

        public AddRoleHandler(ICommandResponseFactory commandResponseFactory, IRoleService roleService, IUserService userService)
        {
            _commandResponseFactory = commandResponseFactory;
            _roleService = roleService;
            _userService = userService;
        }

        public async Task<CommandResponseModel> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            var result = await _roleService.AddRole(request.UserId, request.Role);

            if (result.Succeeded)
            {
                if (request.Role == "Moderator")
                    await _userService.SetUserRank(request.UserId, Rank.Moderator);
                else if (request.Role == "Admin")
                    await _userService.SetUserRank(request.UserId, Rank.Admin);

                return _commandResponseFactory.CreateSuccess();
            }

            var errors = new Dictionary<string, IEnumerable<string>>();
            errors.Add("Auth", result.Errors.Select(error => error.Description));

            return _commandResponseFactory.CreateFailure(errors);
        }
    }
}
