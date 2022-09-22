using VikopApi.Domain.Models;

namespace VikopApi.Domain.Infractructure
{
    public interface ICommentManager
    {
        Task<bool> AddComment(Comment comment);
        Task<bool> AddFindingComment(int commentId, int findingId);
        T GetCommentById<T>(int id, Func<Comment, T> selector);
        Task<bool> AddReaction(CommentReaction reaction);
        Task<bool> DeleteReaction(int commentId, string userId);
        Task<bool> ChangeReaction(CommentReaction newReaction);
        T GetUserReaction<T>(int commentId, string userId, Func<CommentReaction, T> selector);
        Task<bool> AddSubComment(SubComment subComment);
        IEnumerable<T> GetSubComments<T>(int mainCommentId, Func<SubComment, T> selector);
        Task<bool> AddPost(Post post);
        IEnumerable<T> GetPosts<T>(Func<Post, T> selector);
    }
}
