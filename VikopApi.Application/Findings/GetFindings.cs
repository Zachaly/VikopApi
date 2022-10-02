using VikopApi.Application.HelperModels;

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

        public IEnumerable<FindingListItemModel> Execute()
            => _findingManager.GetAllFindings(finding => new FindingListItemModel(finding));
    }
}
