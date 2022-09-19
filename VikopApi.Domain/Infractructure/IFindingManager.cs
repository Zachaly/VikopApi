using VikopApi.Domain.Models;

namespace VikopApi.Domain.Infractructure
{
    public interface IFindingManager
    {
        T GetFindingById<T>(int id, Func<Finding, T> selector);
        IEnumerable<T> GetFindings<T>(Func<Finding, T> selector);
        Task<bool> AddFinding(Finding finding);
        Task<bool> AddReaction(FindingReaction reaction);
        Task<bool> DeleteReaction(int findingId, string userId);
        Task<bool> ChangeReaction(FindingReaction newReaction);
        T GetUserReaction<T>(int findingId, string userId, Func<FindingReaction, T> selector);
    }
}
