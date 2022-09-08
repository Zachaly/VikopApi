
namespace VikopApi.Api.Infrastructure.FileManager
{
    public class FileManager : IFileManager
    {
        private readonly string _profilePicturePath;

        public FileManager(IConfiguration config)
        {
            _profilePicturePath = config["Image:Profile"];
        }

        private FileStream GetFile(string path, string fileName)
            => new FileStream(Path.Combine(path, fileName), FileMode.Open, FileAccess.Read);

        public FileStream GetProfilePicture(string fileName)
            => GetFile(_profilePicturePath, fileName);
    }
}
