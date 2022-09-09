using Microsoft.AspNetCore.Mvc;
using VikopApi.Api.Infrastructure.FileManager;
using VikopApi.Application.Files;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ImageController : ControllerBase
    {
        private IFileManager _fileManager;

        public ImageController(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        [HttpGet("{userId}")]
        public IActionResult ProfilePicture(string userId, [FromServices] GetProfilePicture getProfilePicture)
            => new FileStreamResult(_fileManager.GetProfilePicture(getProfilePicture.Execute(userId)), "image/jpg");

        [HttpGet("{findingId}")]
        public IActionResult FindingPicture(int findingId, [FromServices] GetFindingPicture getFindingPicture)
            => new FileStreamResult(_fileManager.GetFindingPicture(getFindingPicture.Execute(findingId)), "image/jpg");
    }
}
