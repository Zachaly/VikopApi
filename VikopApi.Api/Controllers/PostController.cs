using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Api.DTO;
using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Api.Infrastructure.FileManager;
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
