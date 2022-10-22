using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.User.Abstractions
{
    public interface IUserService
    {
        Task UpdateUser(UpdateUserRequest request);
        UserModel GetUserById(string userId);
        IEnumerable<UserListItemModel> GetUsers();
        IEnumerable<FindingListItemModel> GetUserFindings(string userId);
        IEnumerable<PostModel> GetUserPosts(string userId);
        bool IsEmailOccupied(string email);
        Task<bool> UpdateRanks();
    }
}
