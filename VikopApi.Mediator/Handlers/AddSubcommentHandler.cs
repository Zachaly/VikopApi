using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Comments.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;
using VikopApi.Domain.Models;
using VikopApi.Mediator.Requests;

namespace VikopApi.Mediator.Handlers
{
    public class AddSubcommentHandler : IRequestHandler<AddSubcommentQuery, CommentModel>
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

        public async Task<CommentModel> Handle(AddSubcommentQuery request, CancellationToken cancellationToken)
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
