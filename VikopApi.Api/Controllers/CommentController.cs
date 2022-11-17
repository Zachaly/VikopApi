using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Application.Comments.Abstractions;
using VikopApi.Application.Comments.Commands;
using VikopApi.Application.Models.Comment.Commands;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IMediator _mediator;

        public CommentController(ICommentService commentService, IMediator mediator)
        {
            _commentService = commentService;
            _mediator = mediator;
        }

        /// <summary>
        /// Adds comment to finding
        /// </summary>
        /// <response code="200">
        /// Comment model:
        /// * id
        /// * creatorId
        /// * creatorName
        /// * content
        /// * created - creation date
        /// </response>
        [HttpPost]
        public async Task<IActionResult> AddFindingComment(
            [FromForm] AddFindingCommentCommand request)
        {
            var res = await _mediator.Send(request);

            return Ok(res);
        }
            
        [HttpGet("{commentId}")]
        [AllowAnonymous]
        public IActionResult SubComments(int commentId)
            => Ok(_commentService.GetSubcomments(commentId));

        [HttpPost]
        public async Task<IActionResult> AddSubComment([FromForm] AddSubcommentCommand request)
        {
            var res = await _mediator.Send(request);

            return Ok(res);
        }
    }
}
