using VikopApi.Application.Models.Comment;
using VikopApi.Application.Models.Comment.Requests;

namespace VikopApi.Application.Comments.Abstractions
{
    public interface ICommentFactory
    {
        Comment Create(AddCommentRequest commentRequest);
        CommentModel CreateModel(Comment comment);
        SubComment CreateSubComment(int subCommentId, int mainCommentId);
    }
}
