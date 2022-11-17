using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Command.Abstractions;
using VikopApi.Application.Models.Auth.Commands;
using VikopApi.Application.Models.Command;

namespace VikopApi.Application.Auth.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, DataCommandResponseModel<LoginResponse>>
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICommandResponseFactory _commandResponseFactory;

        public LoginHandler(IAuthService authService, UserManager<ApplicationUser> userManager, ICommandResponseFactory commandResponseFactory)
        {
            _authService = authService;
            _userManager = userManager;
            _commandResponseFactory = commandResponseFactory;
        }

        public async Task<DataCommandResponseModel<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return WrongPasses();
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return WrongPasses();
            }

            var token = await _authService.GetToken(user);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new LoginResponse 
            { 
                Token = tokenJson,
                Claims = (await _userManager.GetClaimsAsync(user)).Select(x => x.Value)
            };

            return _commandResponseFactory.CreateSuccess(response);
        }

        private DataCommandResponseModel<LoginResponse> WrongPasses()
        {
            var errors = new Dictionary<string, IEnumerable<string>>();
            errors.Add("InvalidPasses", new string[] { "Email or password is incorrect!" });

            return _commandResponseFactory.CreateFailure<LoginResponse>(errors);
        }
    }
}
