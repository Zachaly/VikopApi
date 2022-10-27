using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Comments.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.Comments.Commands
{
    public class AddSubcommentCommand : IRequest<CommentModel>
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public IFormFile? Picture { get; set; }
    }

    public class AddSubcommentHandler : IRequestHandler<AddSubcommentCommand, CommentModel>
    {
        private readonly ICommentService _commentService;
        private readonly IAuthService _authService;
        private readonly IFileService _fileService;

        public AddSubcommentHandler(ICommentService commentService, IAuthService authService, IFileService fileService)
        {
            _commentService = commentService;
            _authService = authService;
            _fileService = fileService;
        }

        public async Task<CommentModel> Handle(AddSubcommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new AddSubcommentRequest
            {
                Content = request.Content,
                CreatorId = _authService.GetCurrentUserId(),
                MainCommentId = request.CommentId,
                Picture = ""
            };

            if (request.Picture != null)
            {
                comment.Picture = await _fileService.SaveCommentPicture(request.Picture);
            }

            return await _commentService.AddSubcomment(comment);
        }
    }
}
