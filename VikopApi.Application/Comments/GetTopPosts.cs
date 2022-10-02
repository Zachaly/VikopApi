using VikopApi.Application.HelperModels;

namespace VikopApi.Application.Comments
{
    [Service]
    public class GetTopPosts
    {
        private readonly ICommentManager _commentManager;

        public GetTopPosts(ICommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        public IEnumerable<CommentModel> Execute()
            => _commentManager.GetTopPosts(post => new CommentModel(post.Comment));
    }
}
