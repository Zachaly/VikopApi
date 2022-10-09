using VikopApi.Application.HelperModels;

namespace VikopApi.Application.Posts
{
    [Service]
    public class GetNewPosts
    {
        private readonly IPostManager _postManager;

        public GetNewPosts(IPostManager commentManager)
        {
            _postManager = commentManager;
        }

        public IEnumerable<CommentModel> Execute(int? pageIndex, int? pageSize)
            => _postManager.GetNewPosts(pageIndex ?? 0, pageSize ?? 100,
                post => new CommentModel(post.Comment));
    }
}
