﻿using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Command.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Findings.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.Findings.Commands
{
    public class AddFindingCommand : IRequest<CommandResponseModel>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public IFormFile? Picture { get; set; }
        public string Tags { get; set; }
    }

    public class AddFindingHandler : IRequestHandler<AddFindingCommand, CommandResponseModel>
    {
        private readonly IFindingService _findingService;
        private readonly IAuthService _authService;
        private readonly IFileService _fileService;
        private readonly ICommandResponseFactory _commandResponseFactory;

        public AddFindingHandler(IFindingService findingService, IAuthService authService,
            IFileService fileService, ICommandResponseFactory commandResponseFactory)
        {
            _findingService = findingService;
            _authService = authService;
            _fileService = fileService;
            _commandResponseFactory = commandResponseFactory;
        }

        public async Task<CommandResponseModel> Handle(AddFindingCommand request, CancellationToken cancellationToken)
        {
            await _findingService.AddFinding(new AddFindingRequest
            {
                Title = request.Title,
                CreatorId = _authService.GetCurrentUserId(),
                Link = request.Link,
                Description = request.Description,
                Picture = await _fileService.SaveFindingPicture(request.Picture),
                TagList = request.Tags.Split(',')
            });

            return _commandResponseFactory.CreateSuccess();
        }
    }
}
