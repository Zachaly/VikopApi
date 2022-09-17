
namespace VikopApi.Application.Findings
{
    [Service]
    public class DeleteReaction
    {
        private readonly IFindingManager _findingManager;

        public DeleteReaction(IFindingManager findingManager)
        {
            _findingManager = findingManager;
        }

        public async Task<bool> Execute(int findingId, string userId) 
            => await _findingManager.DeleteReaction(findingId, userId);
    }
}
