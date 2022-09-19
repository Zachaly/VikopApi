
namespace VikopApi.Application.Findings
{
    [Service]
    public class GetUserReaction
    {
        private readonly IFindingManager _findingManager;

        public GetUserReaction(IFindingManager findingManager)
        {
            _findingManager = findingManager;
        }

        public int Execute(int findingId, string userId)
            => _findingManager.GetUserReaction(findingId, userId, reaction => (int?)reaction.Reaction ?? 0);
    }
}
