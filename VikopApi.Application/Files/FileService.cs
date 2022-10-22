using VikopApi.Application.Files.Abstractions;

namespace VikopApi.Application.Files
{
    [Implementation(typeof(IFileService))]
    public class FileService : IFileService
    {
        private readonly IFindingManager _findingManager;
        private readonly ICommentManager _commentManager;
        private readonly IApplicationUserManager _appUserManager;

        public FileService(IFindingManager findingManager, ICommentManager commentManager, IApplicationUserManager applicationUserManager)
        {
            _findingManager = findingManager;
            _commentManager = commentManager;
            _appUserManager = applicationUserManager;
        }

        public string GetCommentPicture(int id)
            => _commentManager.GetCommentById(id, comment => comment.Picture);

        public string GetFindingPicture(int id)
            => _findingManager.GetFindingById(id, finding => finding.Picture);

        public string GetProfilePicture(string id)
            => _appUserManager.GetUserById(id, user => user.ProfilePicture);
    }
}
