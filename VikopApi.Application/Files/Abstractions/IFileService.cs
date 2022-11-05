using Microsoft.AspNetCore.Http;

namespace VikopApi.Application.Files.Abstractions
{
    public interface IFileService
    {
        FileStream GetProfilePicture(string id);
        FileStream GetFindingPicture(int id);
        FileStream GetCommentPicture(int id);
        Task<string> SaveFindingPicture(IFormFile file);
        Task<string> SaveProfilePicture(IFormFile file);
        Task<string> SaveCommentPicture(IFormFile file);
        bool RemoveProfilePicture(string userId);
        bool RemoveFindingPicture(int findingId);
        bool RemoveCommentPicture(int commentId);
    }
}
