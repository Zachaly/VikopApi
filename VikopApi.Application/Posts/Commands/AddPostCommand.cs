using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Command.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Posts.Abstractions;

namespace VikopApi.Application.Posts.Commands
{
    public class AddPostCommand : IRequest<DataCommandResponseModel<PostModel>>
    {
        public string Content { get; set; }
        public IFormFile? Picture { get; set; }
        public string Tags { get; set; }
    }

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
                Tags = request.Tags.Split(',').Select(tag => tag.Replace(" ", "")),
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
