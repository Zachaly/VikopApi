using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopApi.Application.Models;

namespace VikopApi.Application.User.Abstractions
{
    public interface IUserFactory
    {
        public UserModel CreateModel(ApplicationUser user);
        public UserListItemModel CreateListItem(ApplicationUser user);
    }
}
