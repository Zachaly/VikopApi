using VikopApi.Domain.Models;

namespace VikopApi.Domain.Infractructure
{
    public interface ITagManager
    {
        Task<bool> AddTags(IEnumerable<string> names);
        Task<bool> AddTags(IEnumerable<PostTag> tags);
        Task<bool> AddTags(IEnumerable<FindingTag> tags);
        IEnumerable<Tag> GetTagsByNames(IEnumerable<string> names);
    }
}
