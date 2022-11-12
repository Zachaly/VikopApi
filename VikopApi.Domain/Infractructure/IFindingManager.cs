using VikopApi.Domain.Models;

namespace VikopApi.Domain.Infractructure
{
    public interface IFindingManager
    {
        T GetFindingById<T>(int id, Func<Finding, T> selector);
        IEnumerable<T> GetAllFindings<T>(int pageIndex, int pageSize, Func<Finding, T> selector);
        IEnumerable<T> GetNewFindings<T>(int pageIndex, int pageSize, Func<Finding, T> selector);
        IEnumerable<T> GetTopFindings<T>(int pageIndex, int pageSize, Func<Finding, T> selector);
        Task<bool> AddFinding(Finding finding);
        int GetPageCount(int pageSize);
        IEnumerable<T> SearchFindings<T>(int pageIndex, int pageSize, IEnumerable<Func<Finding, bool>> conditions, Func<Finding, T> selector);
        Task<bool> RemoveFindingById(int id);
    }
}
