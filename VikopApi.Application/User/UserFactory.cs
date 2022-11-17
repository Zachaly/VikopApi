using VikopApi.Application.Models.User;
using VikopApi.Application.User.Abstractions;

namespace VikopApi.Application.User
{
    [Implementation(typeof(IUserFactory))]
    public class UserFactory : IUserFactory
    {
        public UserListItemModel CreateListItem(ApplicationUser user)
            => new UserListItemModel
            {
                Id = user.Id,
                Username = user.UserName,
                Rank = user.Rank,
            };

        public UserModel CreateModel(ApplicationUser user)
            => new UserModel
            {
                Id = user.Id,
                Created = user.Created.GetDate(),
                Rank = user.Rank,
                UserName = user.UserName
            };
    }
}
