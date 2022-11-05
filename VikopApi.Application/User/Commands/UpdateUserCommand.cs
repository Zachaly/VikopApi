﻿using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Command.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.User.Abstractions;

namespace VikopApi.Application.User.Commands
{
    public class UpdateUserCommand : IRequest<CommandResponseModel>
    {
        public string Username { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, CommandResponseModel>
    {
        private readonly IAuthService _authService;
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly ICommandResponseFactory _commandResponseFactory;

        public UpdateUserHandler(IAuthService authService, IUserService userService, IFileService fileService,
            ICommandResponseFactory commandResponseFactory)
        {
            _authService = authService;
            _fileService = fileService;
            _userService = userService;
            _commandResponseFactory = commandResponseFactory;
        }

        public async Task<CommandResponseModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
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

            return _commandResponseFactory.CreateSuccess();
        }
    }
}
