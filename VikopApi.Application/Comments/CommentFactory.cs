using VikopApi.Application.Comments.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.Comments
{
    [Implementation(typeof(ICommentFactory))]
    public class CommentFactory : ICommentFactory
    {
        public Comment Create(AddCommentRequest commentRequest)
            => new Comment
            {
                Content = commentRequest.Content,
                CreatorId = commentRequest.CreatorId,
                Created = DateTime.Now,
                Picture = commentRequest.Picture
            };

        public CommentModel CreateModel(Comment comment)
            => new CommentModel(comment);

        public SubComment CreateSubComment(int subCommentId, int mainCommentId)
            => new SubComment
            {
                MainCommentId = mainCommentId,
                CommentId = subCommentId
            };
    }
}
