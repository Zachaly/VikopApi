using VikopApi.Domain.Models;

namespace VikopApi.Domain.Infractructure
{
    public interface ICommentManager
    {
        public Task<bool> AddComment(Comment comment);
        public Task<bool> AddFindingComment(int commentId, int findingId);
        public T GetCommentById<T>(int id, Func<Comment, T> selector);
    }
}
