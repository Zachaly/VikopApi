using VikopApi.Domain.Models;

namespace VikopApi.Domain.Infractructure
{
    public interface IFindingManager
    {
        T GetFindingById<T>(int id, Func<Finding, T> selector);
        IEnumerable<T> GetFindings<T>(Func<Finding, T> selector);
        Task<bool> AddFinding(Finding finding);
    }
}
