using VikopApi.Application.Models;
using VikopApi.Application.Models.Finding;
using VikopApi.Application.Models.Finding.Requests;
using VikopApi.Domain.Enums;

namespace VikopApi.Application.Findings.Abstractions
{
    public interface IFindingService
    {
        Task<int> AddFinding(AddFindingRequest request);
        IEnumerable<FindingListItemModel> GetFindings(PagedRequest request);
        FindingModel GetFindingById(int id);
        int GetPageCount(int pageSize);
        IEnumerable<FindingListItemModel> Search(SearchFindingsRequest request);
        Task<bool> RemoveFindingById(int id);
    }
}
