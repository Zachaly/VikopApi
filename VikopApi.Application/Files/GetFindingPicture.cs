
namespace VikopApi.Application.Files
{
    [Service]
    public class GetFindingPicture
    {
        private readonly IFindingManager _findingManager;

        public GetFindingPicture(IFindingManager findingManager)
        {
            _findingManager = findingManager;
        }

        public string Execute(int id) 
            => _findingManager.GetFindingById(id, finding => finding.Picture);
    }
}
