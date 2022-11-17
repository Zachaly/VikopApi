using VikopApi.Application.Models.Post;

namespace VikopApi.Application.Posts.Abstractions
{
    public interface IPostFactory
    {
        Post Create(Comment comment);
        PostModel CreateModel(Post post);
        PostModel CreateModel(Comment comment, IEnumerable<Tag> tags);
    }
}
