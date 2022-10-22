using VikopApi.Application.Models.Requests;
using VikopApi.Application.Reactions.Abstractions;

namespace VikopApi.Application.Reactions
{
    [Implementation(typeof(IReactionFactory))]
    public class ReactionFactory : IReactionFactory
    {
        public CommentReaction CreateCommentReaction(AddReactionRequest request)
            => new CommentReaction
            {
                CommentId = request.ObjectId,
                Reaction = request.Reaction,
                UserId = request.UserId,
            };

        public FindingReaction CreateFinding(AddReactionRequest request)
            => new FindingReaction
            {
                UserId = request.UserId,
                Reaction = request.Reaction,
                FindingId = request.ObjectId
            };
    }
}
