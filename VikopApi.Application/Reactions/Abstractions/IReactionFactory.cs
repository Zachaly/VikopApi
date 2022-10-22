using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.Reactions.Abstractions
{
    public interface IReactionFactory
    {
        CommentReaction CreateCommentReaction(AddReactionRequest request);
        FindingReaction CreateFinding(AddReactionRequest request);
    }
}
