using VikopApi.Application.HelperModels;

namespace VikopApi.Application.Comments
{
    [Service]
    public class GetPosts
    {
        private readonly ICommentManager _commentManager;

        public GetPosts(ICommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        public IEnumerable<CommentModel> Execute()
            => _commentManager.GetPosts(post => new CommentModel(post.Comment));
    }
}
