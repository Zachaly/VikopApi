using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Database
{
    public class TagManager : ITagManager
    {
        private readonly AppDbContext _dbContext;

        public TagManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddTags(IEnumerable<FindingTag> tags)
        {
            _dbContext.FindingTags.AddRange(tags);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddTags(IEnumerable<PostTag> tags)
        {
            _dbContext.PostTags.AddRange(tags);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddTags(IEnumerable<string> names)
        {
            var notexistingTags = names.Where(name => !_dbContext.Tags.Any(tag => tag.Name == name.ToLower())).ToList();

            if(notexistingTags.Count == 0)
            {
                return true;
            }

            _dbContext.Tags.AddRange(names.Select(name => new Tag { Name = name.ToLower() }));

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public IEnumerable<Tag> GetTagsByNames(IEnumerable<string> names)
            => _dbContext.Tags
                .Where(tag => names.Contains(tag.Name))
                .AsEnumerable();
    }
}
