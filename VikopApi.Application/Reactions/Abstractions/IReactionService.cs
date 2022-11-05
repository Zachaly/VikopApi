using VikopApi.Application.Models.Requests;
using VikopApi.Domain.Enums;

namespace VikopApi.Application.Reactions.Abstractions
{
    public interface IReactionService
    {
        Task<bool> AddCommentReaction(AddReactionRequest request);
        Task<bool> ChangeCommentReaction(AddReactionRequest request);
        Task<bool> AddFindingReaction(AddReactionRequest request);
        Task<bool> ChangeFindingReaction(AddReactionRequest request);
        Task<bool> DeleteCommentReaction(int commentId, string userId);
        Task<bool> DeleteFindingReaction(int findingId, string userId);
        Reaction GetCommentReaction(int commentId, string userId);
        Reaction GetFindingReaction(int findingId, string userId);
    }
}
