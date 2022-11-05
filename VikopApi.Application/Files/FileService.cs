using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using VikopApi.Application.Files.Abstractions;

namespace VikopApi.Application.Files
{
    [Implementation(typeof(IFileService))]
    public class FileService : IFileService
    {
        private readonly IFindingManager _findingManager;
        private readonly ICommentManager _commentManager;
        private readonly IApplicationUserManager _appUserManager;
        private readonly string _profilePicturePath;
        private readonly string _findingPicturePath;
        private readonly string _commentPicturePath;
        private readonly string _placeholderImage;

        public FileService(IFindingManager findingManager,
            ICommentManager commentManager,
            IApplicationUserManager applicationUserManager,
            IConfiguration config)
        {
            _findingManager = findingManager;
            _commentManager = commentManager;
            _appUserManager = applicationUserManager;
            _profilePicturePath = config["Image:Profile"];
            _findingPicturePath = config["Image:Finding"];
            _placeholderImage = config["Image:Placeholder"];
            _commentPicturePath = config["Image:Comment"];
        }

        private string GetCommentPictureName(int id)
            => _commentManager.GetCommentById(id, comment => comment.Picture);

        private string GetFindingPictureName(int id)
            => _findingManager.GetFindingById(id, finding => finding.Picture);

        private string GetProfilePictureName(string id)
            => _appUserManager.GetUserById(id, user => user.ProfilePicture);

        private FileStream GetFile(string path, string fileName)
            => new FileStream(Path.Combine(path, fileName ?? _placeholderImage), FileMode.Open, FileAccess.Read);

        public FileStream GetProfilePicture(string id)
            => GetFile(_profilePicturePath, GetProfilePictureName(id));

        public FileStream GetFindingPicture(int id)
            => GetFile(_findingPicturePath, GetFindingPictureName(id));

        public FileStream GetCommentPicture(int id)
            => GetFile(_commentPicturePath, GetCommentPictureName(id));

        private async Task<string> SaveFile(IFormFile file, string path, string placeholder)
        {
            if (file is null)
            {
                return placeholder;
            }

            try
            {
                Directory.CreateDirectory(path);

                var mime = file.FileName.Substring(file.FileName.LastIndexOf('.'));
                var fileName = $"{Guid.NewGuid()}{mime}";

                using (var stream = File.Create(Path.Combine(path, fileName)))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }
            catch (Exception)
            {
                return placeholder;
            }
        }

        public async Task<string> SaveFindingPicture(IFormFile file)
            => await SaveFile(file, _findingPicturePath, _placeholderImage);

        public async Task<string> SaveProfilePicture(IFormFile file)
            => await SaveFile(file, _profilePicturePath, _placeholderImage);

        public async Task<string> SaveCommentPicture(IFormFile file)
            => await SaveFile(file, _commentPicturePath, "");

        private bool RemoveFile(string? fileName, string path)
        {
            if (fileName == _placeholderImage || fileName is null)
            {
                return true;
            }

            try
            {
                var file = Path.Combine(path, fileName);
                if (File.Exists(file))
                {
                    File.Delete(file);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool RemoveProfilePicture(string userId)
            => RemoveFile(_appUserManager.GetUserById(userId, user => user.ProfilePicture), _profilePicturePath);

        public bool RemoveFindingPicture(int findingId)
            => RemoveFile(_findingManager.GetFindingById(findingId, finding => finding.Picture), _findingPicturePath);

        public bool RemoveCommentPicture(int commentId)
            => RemoveFile(_commentManager.GetCommentById(commentId, comment => comment.Picture), _commentPicturePath);
    }
}
