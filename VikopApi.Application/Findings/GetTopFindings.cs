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

        public IEnumerable<FindingListItemModel> Execute()
            => _findingManager.GetTopFindings(finding => new FindingListItemModel(finding));
    }
}
