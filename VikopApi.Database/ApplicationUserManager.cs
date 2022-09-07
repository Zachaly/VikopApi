using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Database
{
    public class ApplicationUserManager : IApplicationUserManager
    {
        private readonly AppDbContext _dbContext;

        public ApplicationUserManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetUserById<T>(string id, Func<ApplicationUser, T> selector)
            => _dbContext.Users.Where(user => user.Id == id).Select(selector).FirstOrDefault();

        public IEnumerable<T> GetUsers<T>(Func<ApplicationUser, T> selector)
            => _dbContext.Users.Select(selector);
    }
}
