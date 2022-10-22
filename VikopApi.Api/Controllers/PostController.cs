using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Api.Infrastructure.FileManager;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Posts.Abstractions;
using VikopApi.Domain.Enums;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PostController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly int _pageSize;
        private readonly IPostService _postService;
        private readonly IFileManager _fileManager;

        public PostController(IAuthManager authManager, IConfiguration config, IPostService postService, IFileManager fileManager)
        {
            _authManager = authManager;
            _pageSize = int.Parse(config["PageSize"]);
            _postService = postService;
            _fileManager = fileManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost([FromForm] DTO.PostModel post)
        {
            var request = new AddPostRequest
            {
                Content = post.Content,
                CreatorId = _authManager.GetCurrentUserId(),
                Picture = "",
                Tags = post.Tags.Split(','),
            };

            if(post.Picture != null)
            {
                request.Picture = await _fileManager.SaveCommentPicture(post.Picture);
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
