using VikopApi.Application.Models.Reaction.Requests;

namespace VikopApi.Application.Reactions.Abstractions
{
    public interface IReactionFactory
    {
        CommentReaction CreateCommentReaction(AddReactionRequest request);
        FindingReaction CreateFindingReaction(AddReactionRequest request);
    }
}
