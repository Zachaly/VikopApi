
namespace VikopApi.Application.Comments
{
    [Service]
    public class GetUserReaction
    {
        private readonly ICommentManager _commentManager;

        public GetUserReaction(ICommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        public int Execute(int commentId, string userId) 
            => _commentManager.GetUserReaction(commentId, userId, reaction => (int?)reaction.Reaction ?? 0);
    }
}
