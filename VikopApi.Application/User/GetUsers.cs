
namespace VikopApi.Application.User
{
    [Service]
    public class GetUsers
    {
        private readonly IApplicationUserManager _appUserManager;

        public GetUsers(IApplicationUserManager applicationUserManager)
        {
            _appUserManager = applicationUserManager;
        }

        public IEnumerable<UserModel> Execute() 
            => _appUserManager.GetUsers(user => new UserModel
            {
                Id = user.Id,
                Username = user.UserName
            });

        public class UserModel
        {
            public string Id { get; set; }
            public string Username { get; set; }
        }
    }
}
