using VikopApi.Application.Models.Finding;
using VikopApi.Application.Models.Post;
using VikopApi.Application.Models.User;
using VikopApi.Application.Models.User.Requests;
using VikopApi.Domain.Enums;

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
        Task SetUserRank(string userId, Rank rank);
    }
}
