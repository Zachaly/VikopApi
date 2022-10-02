using VikopApi.Application.HelperModels;

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

        public IEnumerable<FindingListItemModel> Execute()
            => _findingManager.GetNewFindings(finding => new FindingListItemModel(finding));
    }
}
