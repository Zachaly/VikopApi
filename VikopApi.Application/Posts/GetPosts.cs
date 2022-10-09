using VikopApi.Application.HelperModels;

namespace VikopApi.Application.Posts
{
    [Service]
    public class GetPosts
    {
        private readonly IPostManager _postManager;

        public GetPosts(IPostManager postManager)
        {
            _postManager = postManager;
        }

        public IEnumerable<CommentModel> Execute(int? pageIndex, int? pageSize)
            => _postManager.GetPosts(pageIndex ?? 0, pageSize ?? 100,
                post => new CommentModel(post.Comment));
    }
}
