using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.Factories.Abstractions
{
    public interface ICommentFactory
    {
        Comment Create(CommentRequest commentRequest);
    }
}
