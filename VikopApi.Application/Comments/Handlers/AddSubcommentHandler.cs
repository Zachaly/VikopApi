using MediatR;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Command.Abstractions;
using VikopApi.Application.Comments.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Models.Command;
using VikopApi.Application.Models.Comment;
using VikopApi.Application.Models.Comment.Commands;
using VikopApi.Application.Models.Comment.Requests;

namespace VikopApi.Application.Comments.Handlers
{
    public class AddSubcommentHandler : IRequestHandler<AddSubcommentCommand, DataCommandResponseModel<CommentModel>>
    {
        private readonly ICommentService _commentService;
        private readonly IAuthService _authService;
        private readonly IFileService _fileService;
        private readonly ICommandResponseFactory _commandResponseFactory;

        public AddSubcommentHandler(ICommentService commentService, IAuthService authService,
            IFileService fileService, ICommandResponseFactory commandResponseFactory)
        {
            _commentService = commentService;
            _authService = authService;
            _fileService = fileService;
            _commandResponseFactory = commandResponseFactory;
        }

        public async Task<DataCommandResponseModel<CommentModel>> Handle(AddSubcommentCommand request, CancellationToken cancellationToken)
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

            var res = await _commentService.AddSubcomment(comment);

            return _commandResponseFactory.CreateSuccess(res);
        }
    }
}
