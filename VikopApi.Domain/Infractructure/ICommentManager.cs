using VikopApi.Domain.Models;

namespace VikopApi.Domain.Infractructure
{
    public interface ICommentManager
    {
        Task<bool> AddComment(Comment comment);
        Task<bool> AddFindingComment(int commentId, int findingId);
        T GetCommentById<T>(int id, Func<Comment, T> selector);
        Task<bool> AddSubComment(SubComment subComment);
        IEnumerable<T> GetSubComments<T>(int mainCommentId, Func<SubComment, T> selector);
    }
}
