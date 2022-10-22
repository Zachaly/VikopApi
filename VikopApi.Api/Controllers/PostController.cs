using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Posts.Abstractions;
using VikopApi.Domain.Enums;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PostController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly int _pageSize;
        private readonly IPostService _postService;
        private readonly IFileService _fileService;

        public PostController(IAuthService authManager, IConfiguration config, IPostService postService, IFileService fileManager)
        {
            _authService = authManager;
            _pageSize = int.Parse(config["PageSize"]);
            _postService = postService;
            _fileService = fileManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost([FromForm] DTO.PostModel post)
        {
            var request = new AddPostRequest
            {
                Content = post.Content,
                CreatorId = _authService.GetCurrentUserId(),
                Picture = "",
                Tags = post.Tags.Split(','),
            };

            if(post.Picture != null)
            {
                request.Picture = await _fileService.SaveCommentPicture(post.Picture);
            }

            return Ok(await _postService.AddPost(request));
        }

        [HttpGet("{pageIndex:int?}")]
        public IActionResult All(int? pageIndex = 0)
            => Ok(_postService.GetPosts(null, pageIndex, _pageSize));

        [HttpGet("{pageIndex:int?}")]
        public IActionResult Hot(int? pageIndex = 0)
            => Ok(_postService.GetPosts(SortingType.Top, pageIndex, _pageSize));

        [HttpGet("{pageIndex:int?}")]
        public IActionResult New(int? pageIndex = 0)
            => Ok(_postService.GetPosts(SortingType.New, pageIndex, _pageSize));

        [HttpGet]
        public IActionResult PageCount()
            => Ok(_postService.GetPageCount(_pageSize));
    }
}
