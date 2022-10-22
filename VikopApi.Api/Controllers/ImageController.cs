using Microsoft.AspNetCore.Mvc;
using VikopApi.Api.Infrastructure.FileManager;
using VikopApi.Application.Files;
using VikopApi.Application.Files.Abstractions;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ImageController : ControllerBase
    {
        private readonly IFileManager _fileManager;
        private readonly IFileService _fileService;

        public ImageController(IFileManager fileManager, IFileService fileService)
        {
            _fileManager = fileManager;
            _fileService = fileService;
        }

        [HttpGet("{userId}")]
        public IActionResult ProfilePicture(string userId)
            => new FileStreamResult(_fileManager.GetProfilePicture(_fileService.GetProfilePicture(userId)), "image/jpg");

        [HttpGet("{findingId}")]
        public IActionResult FindingPicture(int findingId)
            => new FileStreamResult(_fileManager.GetFindingPicture(_fileService.GetFindingPicture(findingId)), "image/jpg");

        [HttpGet("{commentId}")]
        public IActionResult CommentPicture(int commentId)
            => new FileStreamResult(_fileManager.GetCommentPicture(_fileService.GetCommentPicture(commentId)), "image/jpg");
    }
}
