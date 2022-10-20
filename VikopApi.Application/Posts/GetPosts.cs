using VikopApi.Application.Models;

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

        public IEnumerable<PostModel> Execute(int? pageIndex, int? pageSize)
            => _postManager.GetPosts(pageIndex ?? 0, pageSize ?? 100,
                post => new PostModel
                {
                    Content = new CommentModel(post.Comment),
                    TagList = post.Tags.Select(tag => tag.Tag)
                });
    }
}
