using MediatR;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Command.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Models.Command;
using VikopApi.Application.Models.Post;
using VikopApi.Application.Models.Post.Commands;
using VikopApi.Application.Models.Post.Requests;
using VikopApi.Application.Posts.Abstractions;

namespace VikopApi.Application.Posts.Handlers
{
    public class AddPostHandler : IRequestHandler<AddPostCommand, DataCommandResponseModel<PostModel>>
    {
        private readonly IFileService _fileService;
        private readonly IAuthService _authService;
        private readonly IPostService _postService;
        private readonly ICommandResponseFactory _commandResponseFactory;

        public AddPostHandler(IAuthService authService, IFileService fileService,
            IPostService postService, ICommandResponseFactory commandResponseFactory)
        {
            _fileService = fileService;
            _authService = authService;
            _postService = postService;
            _commandResponseFactory = commandResponseFactory;
        }

        public async Task<DataCommandResponseModel<PostModel>> Handle(AddPostCommand request, CancellationToken cancellationToken)
        {
            var postRequest = new AddPostRequest
            {
                Content = request.Content,
                CreatorId = _authService.GetCurrentUserId(),
                Picture = "",
                Tags = request.Tags?.Split(',').Select(tag => tag.Replace(" ", "")) ?? Array.Empty<string>(),
            };

            if (request.Picture != null)
            {
                postRequest.Picture = await _fileService.SaveCommentPicture(request.Picture);
            }

            var res = await _postService.AddPost(postRequest);

            return _commandResponseFactory.CreateSuccess(res);
        }
    }
}
