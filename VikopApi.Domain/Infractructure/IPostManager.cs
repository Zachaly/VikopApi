using VikopApi.Domain.Models;

namespace VikopApi.Domain.Infractructure
{
    public interface IPostManager
    {
        Task<bool> AddPost(Post post);
        IEnumerable<T> GetPosts<T>(int pageIndex, int pageSize, Func<Post, T> selector);
        IEnumerable<T> GetTopPosts<T>(int pageIndex, int pageSize, Func<Post, T> selector);
        IEnumerable<T> GetNewPosts<T>(int pageIndex, int pageSize, Func<Post, T> selector);
        int GetPageCount(int pageSize);
        IEnumerable<T> SearchPosts<T>(int pageIndex, int pageSize, IEnumerable<Func<Post, bool>> conditions, Func<Post, T> selector);
    }
}
