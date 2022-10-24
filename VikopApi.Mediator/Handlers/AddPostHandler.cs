using MediatR;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Posts.Abstractions;
using VikopApi.Mediator.Requests;

namespace VikopApi.Mediator.Handlers
{
    public class AddPostHandler : IRequestHandler<AddPostQuery, PostModel>
    {
        private readonly IFileService _fileService;
        private readonly IAuthService _authService;
        private readonly IPostService _postService;

        public AddPostHandler(IAuthService authService, IFileService fileService, IPostService postService)
        {
            _fileService = fileService;
            _authService = authService;
            _postService = postService;
        }

        public async Task<PostModel> Handle(AddPostQuery request, CancellationToken cancellationToken)
        {
            var postRequest = new AddPostRequest
            {
                Content = request.Content,
                CreatorId = _authService.GetCurrentUserId(),
                Picture = "",
                Tags = request.Tags.Split(','),
            };

            if (request.Picture != null)
            {
                postRequest.Picture = await _fileService.SaveCommentPicture(request.Picture);
            }

            return await _postService.AddPost(postRequest);
        }
    }
}
