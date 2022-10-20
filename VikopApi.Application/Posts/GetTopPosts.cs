using VikopApi.Application.Models;

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

        public IEnumerable<PostModel> Execute(int? pageIndex, int? pageSize)
            => _postManager.GetTopPosts(pageIndex ?? 0, pageSize ?? 100,
                post => new PostModel
                {
                    Content = new CommentModel(post.Comment),
                    TagList = post.Tags.Select(tag => tag.Tag)
                });
    }
}
