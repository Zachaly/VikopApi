
namespace VikopApi.Api.Infrastructure.FileManager
{
    public class FileManager : IFileManager
    {
        private readonly string _profilePicturePath;
        private readonly string _findingPicturePath;
        private readonly string _placeholderImage;

        public FileManager(IConfiguration config)
        {
            _profilePicturePath = config["Image:Profile"];
            _findingPicturePath = config["Image:Finding"];
            _placeholderImage = config["Image:Placeholder"];
        }

        private FileStream GetFile(string path, string fileName)
            => new FileStream(Path.Combine(path, fileName), FileMode.Open, FileAccess.Read);

        public FileStream GetProfilePicture(string fileName)
            => GetFile(_profilePicturePath, fileName);

        public FileStream GetFindingPicture(string fileName)
            => GetFile(_findingPicturePath, fileName);

        private async Task<string> SaveFile(IFormFile file, string path)
        {
            if(file is null)
            {
                return _placeholderImage;
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
                return _placeholderImage;
            }
        }

        public async Task<string> SaveFindingPicture(IFormFile file)
            => await SaveFile(file, _findingPicturePath);

        public async Task<string> SaveProfilePicture(IFormFile file)
            => await SaveFile(file, _profilePicturePath);

        private bool RemoveFile(string fileName, string path)
        {
            if(fileName == _placeholderImage)
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

        public bool RemoveProfilePicture(string fileName)
            => RemoveFile(fileName, _profilePicturePath);

        public bool RemoveFindingPicture(string fileName)
            => RemoveFile(fileName, _findingPicturePath);
    }
}
