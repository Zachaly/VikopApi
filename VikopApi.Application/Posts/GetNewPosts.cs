using VikopApi.Application.Models;

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

        public IEnumerable<PostModel> Execute(int? pageIndex, int? pageSize)
            => _postManager.GetNewPosts(pageIndex ?? 0, pageSize ?? 100,
                post => new PostModel
                {
                    Content = new CommentModel(post.Comment),
                    TagList = post.Tags.Select(tag => tag.Tag)
                });
    }
}
