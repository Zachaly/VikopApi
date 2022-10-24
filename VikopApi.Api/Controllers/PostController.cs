using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Application.Posts.Abstractions;
using VikopApi.Domain.Enums;
using VikopApi.Mediator.Requests;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PostController : ControllerBase
    {
        private readonly int _pageSize;
        private readonly IPostService _postService;
        private readonly IMediator _mediator;

        public PostController(IConfiguration config, IPostService postService, IMediator mediator)
        {
            _pageSize = int.Parse(config["PageSize"]);
            _postService = postService;
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost([FromForm] AddPostQuery request)
        {
            var res = await _mediator.Send(request);

            return Ok(res);
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
