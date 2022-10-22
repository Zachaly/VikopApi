using VikopApi.Application.Tags.Abtractions;

namespace VikopApi.Application.Tags
{
    [Implementation(typeof(ITagFactory))]
    public class TagFactory : ITagFactory
    {
        public IEnumerable<Tag> Create(IEnumerable<string> tags)
            => tags.Select(tag => new Tag
            {
                Name = tag
            });

        public IEnumerable<FindingTag> CreateFinding(int findingId, IEnumerable<Tag> tags)
            => tags.Select(tag => new FindingTag
            {
                FindingId = findingId,
                TagId = tag.Id
            });

        public IEnumerable<PostTag> CreatePost(int postId, IEnumerable<Tag> tags)
            => tags.Select(tag => new PostTag
            {
                PostId = postId,
                TagId = tag.Id
            });
    }
}
