using VikopApi.Application.Comments.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Posts.Abstractions;

namespace VikopApi.Application.Posts
{
    [Implementation(typeof(IPostFactory))]
    public class PostFactory : IPostFactory
    {
        private readonly ICommentFactory _commentFactory;

        public PostFactory(ICommentFactory commentFactory)
        {
            _commentFactory = commentFactory;
        }

        public Post Create(Comment comment)
            => new Post
            {
                CommentId = comment.Id
            };

        public PostModel CreateModel(Post post)
            => new PostModel
            {
                Content = _commentFactory.CreateModel(post.Comment),
                TagList = post.Tags.Select(tag => tag.Tag)
            };

        public PostModel CreateModel(Comment comment, IEnumerable<Tag> tags)
            => new PostModel
            {
                Content = _commentFactory.CreateModel(comment),
                TagList = tags
            };
    }
}
