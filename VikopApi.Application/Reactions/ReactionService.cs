using VikopApi.Application.Models.Requests;
using VikopApi.Application.Reactions.Abstractions;
using VikopApi.Domain.Enums;

namespace VikopApi.Application.Reactions
{
    [Implementation(typeof(IReactionService))]
    public class ReactionService : IReactionService
    {
        private IReactionFactory _reactionFactory;
        private readonly IReactionManager _reactionManager;

        public ReactionService(IReactionFactory reactionFactory, IReactionManager reactionManager)
        {
            _reactionFactory = reactionFactory;
            _reactionManager = reactionManager;
        }

        public async Task<bool> AddCommentReaction(AddReactionRequest request)
            => await _reactionManager.AddReaction(_reactionFactory.CreateCommentReaction(request));

        public async Task<bool> AddFindingReaction(AddReactionRequest request)
            => await _reactionManager.AddReaction(_reactionFactory.CreateFindingReaction(request));

        public async Task<bool> ChangeCommentReaction(AddReactionRequest request)
            => await _reactionManager.ChangeReaction(_reactionFactory.CreateCommentReaction(request));

        public async Task<bool> ChangeFindingReaction(AddReactionRequest request)
            => await _reactionManager.ChangeReaction(_reactionFactory.CreateFindingReaction(request));

        public async Task<bool> DeleteCommentReaction(int commentId, string userId)
            => await _reactionManager.DeleteCommentReaction(commentId, userId);

        public async Task<bool> DeleteFindingReaction(int findingId, string userId)
            => await _reactionManager.DeleteFindingReaction(findingId, userId);

        public Reaction GetCommentReaction(int commentId, string userId)
            => _reactionManager.GetUserCommentReaction(userId, commentId, reaction => reaction.Reaction);

        public Reaction GetFindingReaction(int findingId, string userId)
            => _reactionManager.GetUserFindingReaction(userId, findingId, reaction => reaction.Reaction);
    }
}
