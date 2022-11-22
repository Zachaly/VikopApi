using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Post.Commands;
using VikopApi.Application.Models.Post.Requests;
using VikopApi.Application.Models.Post.Validation;
using VikopApi.Application.Posts.Abstractions;
using VikopApi.Domain.Enums;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMediator _mediator;

        public PostController(IPostService postService, IMediator mediator)
        {
            _postService = postService;
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(
            [FromForm] AddPostCommand request,
            [FromServices] AddPostValidator validator)
        {
            var validation = validator.Validate(request);
            if (!validation.IsValid)
            {
                return BadRequest(validation.Errors);
            }

            var res = await _mediator.Send(request);

            return Ok(res);
        }

        [HttpGet]
        public IActionResult Get([FromQuery] PagedRequest request)
            => Ok(_postService.GetPosts(request));

        [HttpGet("pagecount/{pageSize}")]
        public IActionResult PageCount(int pageSize)
            => Ok(_postService.GetPageCount(pageSize));

        [HttpGet("search")]
        public IActionResult Search([FromQuery] SearchPostRequest request)
            => Ok(_postService.Search(request));
    }
}
