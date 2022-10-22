using VikopApi.Application.Models.Requests;
using VikopApi.Domain.Enums;

namespace VikopApi.Application.Reactions.Abstractions
{
    public interface IReactionService
    {
        Task AddCommentReaction(AddReactionRequest request);
        Task ChangeCommentReaction(AddReactionRequest request);
        Task AddFindingReaction(AddReactionRequest request);
        Task ChangeFindingReaction(AddReactionRequest request);
        Task DeleteCommentReaction(int commentId, string userId);
        Task DeleteFindingReaction(int findingId, string userId);
        Reaction GetCommentReaction(int commentId, string userId);
        Reaction GetFindingReaction(int findingId, string userId);
    }
}
