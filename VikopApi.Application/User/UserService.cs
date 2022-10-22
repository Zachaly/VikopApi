using VikopApi.Application.Findings.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Posts.Abstractions;
using VikopApi.Application.User.Abstractions;

namespace VikopApi.Application.User
{
    [Implementation(typeof(IUserService))]
    public class UserService : IUserService
    {
        private IApplicationUserManager _appUserManager;
        private IUserFactory _userFactory;
        private readonly IFindingFactory _findingFactory;
        private readonly IPostFactory _postFactory;

        public UserService(IApplicationUserManager applicationUserManager,
            IUserFactory userFactory,
            IFindingFactory findingFactory,
            IPostFactory postFactory)
        {
            _appUserManager = applicationUserManager;
            _userFactory = userFactory;
            _findingFactory = findingFactory;
            _postFactory = postFactory;
        }

        public UserModel GetUserById(string userId)
            => _appUserManager.GetUserById(userId, user => _userFactory.CreateModel(user));

        public IEnumerable<FindingListItemModel> GetUserFindings(string userId)
            => _appUserManager.GetUserFindings(userId, finding => _findingFactory.CreateListItem(finding));

        public IEnumerable<PostModel> GetUserPosts(string userId)
            => _appUserManager.GetUserPosts(userId, post => _postFactory.CreateModel(post));

        public IEnumerable<UserListItemModel> GetUsers()
            => _appUserManager.GetUsers(user => _userFactory.CreateListItem(user));

        public bool IsEmailOccupied(string email)
            => _appUserManager.GetUsers(user => user.Email.ToUpper())
                .Any(userEmail => userEmail == email.ToUpper());

        public Task<bool> UpdateRanks()
            => _appUserManager.UpdateRanks();

        public Task UpdateUser(UpdateUserRequest request)
            => _appUserManager.UpdateUser(request.Id, user =>
            {
                user.UserName = request.UserName;
                if (!string.IsNullOrEmpty(request.Picture))
                {
                    user.ProfilePicture = request.Picture;
                }
            });
    }
}
