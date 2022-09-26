
namespace VikopApi.Api.Infrastructure.FileManager
{
    public interface IFileManager
    {
        FileStream GetProfilePicture(string fileName);
        FileStream GetFindingPicture(string fileName);
        Task<string> SaveFindingPicture(IFormFile file);
        Task<string> SaveProfilePicture(IFormFile file);
        bool RemoveProfilePicture(string fileName);
        bool RemoveFindingPicture(string fileName);
    }
}
