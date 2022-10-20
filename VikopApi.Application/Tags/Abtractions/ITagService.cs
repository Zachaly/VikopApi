namespace VikopApi.Application.Tags.Abtractions
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> CreatePost(IEnumerable<string> names, int id);
        Task<IEnumerable<Tag>> CreateFinding(IEnumerable<string> names, int id);
    }
}
