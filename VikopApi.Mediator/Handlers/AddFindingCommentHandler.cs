using MediatR;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Comments.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;
using VikopApi.Mediator.Requests;

namespace VikopApi.Mediator.Handlers
{
    public class AddFindingCommentHandler : IRequestHandler<AddFindingCommentQuery, CommentModel>
    {
        private readonly IFileService _fileService;
        private readonly IAuthService _authService;
        private readonly ICommentService _commentService;

        public AddFindingCommentHandler(IAuthService authService, ICommentService commentService, IFileService fileService)
        {
            _fileService = fileService;
            _authService = authService;
            _commentService = commentService;
        }

        public async Task<CommentModel> Handle(AddFindingCommentQuery request, CancellationToken cancellationToken)
        {
            var commentRequest = new AddFindingCommentRequest
            {
                Content = request.Content,
                CreatorId = _authService.GetCurrentUserId(),
                FindingId = request.FindingId,
                Picture = ""
            };

            if (request.Picture != null)
            {
                commentRequest.Picture = await _fileService.SaveCommentPicture(request.Picture);
            }

            return await _commentService.AddFindingComment(commentRequest);
        }
    }
}
