namespace VikopApi.Application.User
{
    [Service]
    public class UpdateRanks
    {
        private readonly IApplicationUserManager _appUserManager;

        public UpdateRanks(IApplicationUserManager applicationUserManager)
        {
            _appUserManager = applicationUserManager;
        }

        public async Task<bool> Execute() => await _appUserManager.UpdateRanks();
    }
}
