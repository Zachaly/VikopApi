
namespace VikopApi.Api.Infrastructure.FileManager
{
    public class FileManager : IFileManager
    {
        private readonly string _profilePicturePath;
        private readonly string _findingPicturePath;

        public FileManager(IConfiguration config)
        {
            _profilePicturePath = config["Image:Profile"];
            _findingPicturePath = config["Image:Finding"];
        }

        private FileStream GetFile(string path, string fileName)
            => new FileStream(Path.Combine(path, fileName), FileMode.Open, FileAccess.Read);

        public FileStream GetProfilePicture(string fileName)
            => GetFile(_profilePicturePath, fileName);

        public FileStream GetFindingPicture(string fileName)
            => GetFile(_findingPicturePath, fileName);
    }
}
