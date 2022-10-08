using VikopApi.Application.HelperModels;

namespace VikopApi.Application.Findings
{
    [Service]
    public class GetTopFindings
    {
        private readonly IFindingManager _findingManager;

        public GetTopFindings(IFindingManager findingManager)
        {
            _findingManager = findingManager;
        }

        public IEnumerable<FindingListItemModel> Execute(int? pageIndex, int? pageSize)
            => _findingManager.GetTopFindings(pageIndex ?? 0, pageSize ?? 100, 
                finding => new FindingListItemModel(finding));
    }
}
