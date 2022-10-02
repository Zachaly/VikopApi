using VikopApi.Application.HelperModels;

namespace VikopApi.Application.Comments
{
    [Service]
    public class GetNewPosts
    {
        private readonly ICommentManager _commentManager;

        public GetNewPosts(ICommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        public IEnumerable<CommentModel> Execute()
            => _commentManager.GetNewPosts(post => new CommentModel(post.Comment));
    }
}
