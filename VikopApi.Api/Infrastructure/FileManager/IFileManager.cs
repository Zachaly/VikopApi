
namespace VikopApi.Api.Infrastructure.FileManager
{
    public interface IFileManager
    {
        FileStream GetProfilePicture(string fileName);
    }
}
