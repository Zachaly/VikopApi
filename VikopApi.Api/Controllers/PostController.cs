using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Api.DTO;
using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Application.Comments;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PostController : ControllerBase
    {
        private readonly IAuthManager _authManager;

        public PostController(IAuthManager authManager)
        {
            _authManager = authManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost(
            PostModel post,
            [FromServices] AddPost addPost)
        {
            var request = new AddPost.Request
            {
                Content = post.Content,
                CreatorId = _authManager.GetCurrentUserId()
            };

            return Ok(await addPost.Execute(request));
        }

        [HttpGet]
        public IActionResult All([FromServices] GetPosts getPosts)
            => Ok(getPosts.Execute());

        [HttpGet]
        public IActionResult Hot([FromServices] GetTopPosts getTopPosts)
            => Ok(getTopPosts.Execute());

        [HttpGet]
        public IActionResult New([FromServices] GetNewPosts getNewPosts)
            => Ok(getNewPosts.Execute());
    }
}
