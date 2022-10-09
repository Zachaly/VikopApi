using VikopApi.Application.HelperModels;

namespace VikopApi.Application.Posts
{
    [Service]
    public class GetTopPosts
    {
        private readonly IPostManager _postManager;

        public GetTopPosts(IPostManager postManager)
        {
            _postManager = postManager;
        }

        public IEnumerable<CommentModel> Execute(int? pageIndex, int? pageSize)
            => _postManager.GetTopPosts(pageIndex ?? 0, pageSize ?? 100,
                post => new CommentModel(post.Comment));
    }
}
