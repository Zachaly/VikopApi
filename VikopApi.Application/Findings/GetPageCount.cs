namespace VikopApi.Application.Findings
{
    [Service]
    public class GetPageCount
    {
        private readonly IFindingManager _findingManager;

        public GetPageCount(IFindingManager findingManager)
        {
            _findingManager = findingManager;
        }

        public int Execute(int pageSize) => _findingManager.GetPageCount(pageSize);
    }
}
