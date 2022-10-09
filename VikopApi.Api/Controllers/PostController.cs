using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Api.DTO;
using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Api.Infrastructure.FileManager;
using VikopApi.Application.Posts;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PostController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly int _pageSize;

        public PostController(IAuthManager authManager, IConfiguration config)
        {
            _authManager = authManager;
            _pageSize = int.Parse(config["PageSize"]);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost(
            [FromForm] PostModel post,
            [FromServices] AddPost addPost,
            [FromServices] IFileManager fileManager)
        {
            var request = new AddPost.Request
            {
                Content = post.Content,
                CreatorId = _authManager.GetCurrentUserId(),
                Picture = ""
            };

            if(post.Picture != null)
            {
                request.Picture = await fileManager.SaveCommentPicture(post.Picture);
            }

            return Ok(await addPost.Execute(request));
        }

        [HttpGet("{pageIndex:int?}")]
        public IActionResult All([FromServices] GetPosts getPosts, int? pageIndex = 0)
            => Ok(getPosts.Execute(pageIndex, _pageSize));

        [HttpGet("{pageIndex:int?}")]
        public IActionResult Hot([FromServices] GetTopPosts getTopPosts, int? pageIndex = 0)
            => Ok(getTopPosts.Execute(pageIndex, _pageSize));

        [HttpGet("{pageIndex:int?}")]
        public IActionResult New([FromServices] GetNewPosts getNewPosts, int? pageIndex = 0)
            => Ok(getNewPosts.Execute(pageIndex, _pageSize));

        [HttpGet]
        public IActionResult PageCount([FromServices] GetPageCount getPageCount)
            => Ok(getPageCount.Execute(_pageSize));
    }
}
