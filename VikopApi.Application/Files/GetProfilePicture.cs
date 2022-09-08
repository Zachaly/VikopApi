using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikopApi.Application.Files
{
    [Service]
    public class GetProfilePicture
    {
        private IApplicationUserManager _appUserManager;

        public GetProfilePicture(IApplicationUserManager applicationUserManager)
        {
            _appUserManager = applicationUserManager;
        }

        public string Execute(string userId) => _appUserManager.GetUserById(userId, user => user.ProfilePicture);
    }
}
