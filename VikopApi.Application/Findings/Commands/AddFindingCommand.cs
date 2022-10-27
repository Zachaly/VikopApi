using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Findings.Abstractions;
using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.Findings.Commands
{
    public class AddFindingCommand : IRequest<Unit>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public IFormFile? Picture { get; set; }
        public string Tags { get; set; }
    }

    public class AddFindingHandler : IRequestHandler<AddFindingCommand>
    {
        private readonly IFindingService _findingService;
        private readonly IAuthService _authService;
        private readonly IFileService _fileService;

        public AddFindingHandler(IFindingService findingService, IAuthService authService, IFileService fileService)
        {
            _findingService = findingService;
            _authService = authService;
            _fileService = fileService;
        }

        public async Task<Unit> Handle(AddFindingCommand request, CancellationToken cancellationToken)
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

            return Unit.Value;
        }
    }
}
