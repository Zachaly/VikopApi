using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Domain.Models;
using VikopApi.Mediator.Requests;
using VikopApi.Mediator.Responses;

namespace VikopApi.Mediator.Handlers
{
    public class LoginHandler : IRequestHandler<LoginQuery, LoginResponse>
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;

        public LoginHandler(IAuthService authService, UserManager<ApplicationUser> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }

        public async Task<LoginResponse> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new LoginResponse { Error = true, Errors = new string[] { "Email or password is incorrect!" } };
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return new LoginResponse { Error = true, Errors = new string[] { "Email or password is incorrect!" } };
            }

            var token = await _authService.GetToken(user);

            var tokenJson = new JwtSecurityTokenHandler().WriteToken(token);

            return new LoginResponse { Token = tokenJson };
        }
    }
}
