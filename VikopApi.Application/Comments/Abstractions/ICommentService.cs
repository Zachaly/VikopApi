using VikopApi.Application.Models.Comment;
using VikopApi.Application.Models.Comment.Requests;

namespace VikopApi.Application.Comments.Abstractions
{
    public interface ICommentService
    {
        Task<CommentModel> AddFindingComment(AddFindingCommentRequest request);
        CommentModel GetCommentById(int id);
        IEnumerable<CommentModel> GetSubcomments(int commentId);
        Task<CommentModel> AddSubcomment(AddSubcommentRequest request);
    }
}
