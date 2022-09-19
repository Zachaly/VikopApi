
namespace VikopApi.Application.Comments
{
    [Service]
    public class DeleteReaction
    {
        private readonly ICommentManager _commentManager;

        public DeleteReaction(ICommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        public async Task<bool> Execute(int commentId, string userId)
            => await _commentManager.DeleteReaction(commentId, userId);
    }
}
