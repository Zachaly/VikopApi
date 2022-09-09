
namespace VikopApi.Api.Infrastructure.FileManager
{
    public interface IFileManager
    {
        FileStream GetProfilePicture(string fileName);
        FileStream GetFindingPicture(string fileName);
    }
}
