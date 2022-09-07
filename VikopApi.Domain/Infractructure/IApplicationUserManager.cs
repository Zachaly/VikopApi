using VikopApi.Domain.Models;

namespace VikopApi.Domain.Infractructure
{
    public interface IApplicationUserManager
    {
        IEnumerable<T> GetUsers<T>(Func<ApplicationUser, T> selector);
        T GetUserById<T>(string id, Func<ApplicationUser, T> selector);
    }
}
