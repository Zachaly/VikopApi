using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.Comments.Abstractions
{
    public interface ICommentFactory
    {
        Comment Create(AddCommentRequest commentRequest);
        CommentModel CreateModel(Comment comment);
        SubComment CreateSubComment(int subCommentId, int mainCommentId);
    }
}
