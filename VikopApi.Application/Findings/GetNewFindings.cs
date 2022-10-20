using VikopApi.Application.Models;

namespace VikopApi.Application.Findings
{
    [Service]
    public class GetNewFindings
    {
        private readonly IFindingManager _findingManager;

        public GetNewFindings(IFindingManager findingManager)
        {
            _findingManager = findingManager;
        }

        public IEnumerable<FindingListItemModel> Execute(int? pageIndex, int? pageSize)
            => _findingManager.GetNewFindings(pageIndex ?? 0, pageSize ?? 100, 
                finding => new FindingListItemModel(finding));
    }
}
