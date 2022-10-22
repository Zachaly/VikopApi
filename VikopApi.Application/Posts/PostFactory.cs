using VikopApi.Application.Models;
using VikopApi.Application.Posts.Abstractions;

namespace VikopApi.Application.Posts
{
    [Implementation(typeof(IPostFactory))]
    public class PostFactory : IPostFactory
    {
        public Post Create(Comment comment)
            => new Post
            {
                CommentId = comment.Id
            };

        public PostModel CreateModel(Post post)
            => new PostModel
            {
                Content = new CommentModel(post.Comment),
                TagList = post.Tags.Select(tag => tag.Tag)
            };

        public PostModel CreateModel(Comment comment, IEnumerable<Tag> tags)
            => new PostModel
            {
                Content = new CommentModel(comment),
                TagList = tags
            };
    }
}
