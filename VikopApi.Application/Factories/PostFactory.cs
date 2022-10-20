using VikopApi.Application.Factories.Abstractions;

namespace VikopApi.Application.Factories
{
    public class PostFactory : IPostFactory
    {
        public Post Create(Comment comment)
            => new Post
            {
                CommentId = comment.Id
            };
    }
}
