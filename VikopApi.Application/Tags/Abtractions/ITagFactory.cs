namespace VikopApi.Application.Tags.Abtractions
{
    public interface ITagFactory
    {
        IEnumerable<Tag> Create(IEnumerable<string> tags);
        IEnumerable<PostTag> CreatePost(int postId, IEnumerable<Tag> tags);
        IEnumerable<FindingTag> CreateFinding(int findingId, IEnumerable<Tag> tags);
    }
}
