using Microsoft.AspNetCore.Mvc;
using VikopApi.Application.Files.Abstractions;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ImageController : ControllerBase
    {
        private readonly IFileService _fileService;

        public ImageController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet("{userId}")]
        public IActionResult ProfilePicture(string userId)
            => new FileStreamResult(_fileService.GetProfilePicture(userId), "image/jpg");

        [HttpGet("{findingId}")]
        public IActionResult FindingPicture(int findingId)
            => new FileStreamResult(_fileService.GetFindingPicture(findingId), "image/jpg");

        [HttpGet("{commentId}")]
        public IActionResult CommentPicture(int commentId)
            => new FileStreamResult(_fileService.GetCommentPicture(commentId), "image/jpg");
    }
}
