using VikopApi.Application.Models;

namespace VikopApi.Application.User
{
    [Service]
    public class GetUserFindings
    {
        private IApplicationUserManager _appUserManager;

        public GetUserFindings(IApplicationUserManager applicationUserManager)
        {
            _appUserManager = applicationUserManager;
        }

        public IEnumerable<FindingListItemModel> Execute(string id)
            => _appUserManager.GetUserFindings(id, finding => new FindingListItemModel(finding));
    }
}
