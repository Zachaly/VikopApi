using VikopApi.Application.Models;

namespace VikopApi.Application.Findings
{
    [Service]
    public class GetFindings
    {
        private readonly IFindingManager _findingManager;

        public GetFindings(IFindingManager findingManager)
        {
            _findingManager = findingManager;
        }

        public IEnumerable<FindingListItemModel> Execute(int? pageIndex, int? pageSize)
            => _findingManager.GetAllFindings(pageIndex ?? 0, pageSize ?? 100,
                finding => new FindingListItemModel(finding));
    }
}
