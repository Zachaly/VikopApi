namespace VikopApi.Application.User
{
    [Service]
    public class IsEmailOccupied
    {
        private IApplicationUserManager _appUserManager;

        public IsEmailOccupied(IApplicationUserManager applicationUserManager)
        {
            _appUserManager = applicationUserManager;
        }

        public bool Execute(string email)
            => _appUserManager.GetUsers(user => user.NormalizedEmail)
                .Any(userEmail => email.ToUpper() == userEmail);
    }
}
