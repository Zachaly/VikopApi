using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopApi.Domain.Models;

namespace VikopApi.Domain.Infractructure
{
    public interface IReportManager
    {
        Task<bool> AddReport(FindingReport report);
        Task<bool> AddReport(PostReport report);
        Task<bool> RemovePostReport(int postId);
        Task<bool> RemoveFindingReport(int findingId);
        IEnumerable<T> GetReports<T>(int pageIndex, int pageSize, Func<PostReport, T> selector);
        IEnumerable<T> GetReports<T>(int pageIndex, int pageSize, Func<FindingReport, T> selector);
        T GetReportById<T>(int id, Func<PostReport, T> selector);
        T GetReportById<T>(int id, Func<FindingReport, T> selector);
    }
}
