namespace VikopApi.Api.Infrastructure.FileManager
{
    public interface IFileManager
    {
        FileStream GetProfilePicture(string fileName);
        FileStream GetFindingPicture(string fileName);
        FileStream GetCommentPicture(string fileName);
        Task<string> SaveFindingPicture(IFormFile file);
        Task<string> SaveProfilePicture(IFormFile file);
        Task<string> SaveCommentPicture(IFormFile file);
        bool RemoveProfilePicture(string fileName);
        bool RemoveFindingPicture(string fileName);
        bool RemoveCommentPicture(string fileName);
    }
}
