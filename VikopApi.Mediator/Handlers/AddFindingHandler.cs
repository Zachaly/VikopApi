using MediatR;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Findings.Abstractions;
using VikopApi.Application.Models.Requests;
using VikopApi.Mediator.Requests;

namespace VikopApi.Mediator.Handlers
{
    public class AddFindingHandler : IRequestHandler<AddFindingQuery>
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

        public async Task<Unit> Handle(AddFindingQuery request, CancellationToken cancellationToken)
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
