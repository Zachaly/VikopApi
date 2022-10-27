using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Comments.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.Comments.Commands
{
    public class AddFindingCommentCommand : IRequest<CommentModel>
    {
        public int FindingId { get; set; }
        public string Content { get; set; }
        public IFormFile? Picture { get; set; }
    }

    public class AddFindingCommentHandler : IRequestHandler<AddFindingCommentCommand, CommentModel>
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

        public async Task<CommentModel> Handle(AddFindingCommentCommand request, CancellationToken cancellationToken)
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
