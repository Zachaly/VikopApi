using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;

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
