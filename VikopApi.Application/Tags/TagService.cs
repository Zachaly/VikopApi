using VikopApi.Application.Tags.Abtractions;

namespace VikopApi.Application.Tags
{
    [Implementation(typeof(ITagService))]
    public class TagService : ITagService
    {
        private readonly ITagManager _tagManager;
        private readonly ITagFactory _tagFactory;

        public TagService(ITagManager tagManager, ITagFactory tagFactory)
        {
            _tagManager = tagManager;
            _tagFactory = tagFactory;
        }

        private async Task<IEnumerable<Tag>> Create(IEnumerable<string> names)
        {
            await _tagManager.AddTags(names);

            return _tagManager.GetTagsByNames(names);
        }

        public async Task<IEnumerable<Tag>> CreatePost(IEnumerable<string> names, int id)
        {
            var tags = await Create(names);

            var postTags = _tagFactory.CreatePost(id, tags);

            await _tagManager.AddTags(postTags);

            return tags;
        }

        public async Task<IEnumerable<Tag>> CreateFinding(IEnumerable<string> names, int id)
        {
            var tags = await Create(names);

            var postTags = _tagFactory.CreateFinding(id, tags);

            await _tagManager.AddTags(postTags);

            return tags;
        }
    }
}
