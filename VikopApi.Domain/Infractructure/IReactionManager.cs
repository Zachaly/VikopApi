using VikopApi.Domain.Models;

namespace VikopApi.Domain.Infractructure
{
    public interface IReactionManager
    {
        T GetUserCommentReaction<T>(string userId, int commentId, Func<CommentReaction, T> selector);
        T GetUserFindingReaction<T>(string userId, int findingId, Func<FindingReaction, T> selector);
        Task<bool> AddReaction(CommentReaction reaction);
        Task<bool> AddReaction(FindingReaction reaction);
        Task<bool> ChangeReaction(FindingReaction reaction);
        Task<bool> ChangeReaction(CommentReaction reaction);
        Task<bool> DeleteCommentReaction(int commentId, string userId);
        Task<bool> DeleteFindingReaction(int findingId, string userId);
    }
}
