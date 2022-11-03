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
            => new CommentModel
            {
                Content = comment.Content,
                Created = comment.Created.GetTime(),
                CreatorId = comment.CreatorId,
                CreatorName = comment.Creator.UserName,
                CreatorRank = (int)comment.Creator.Rank,
                Id = comment.Id,
                Reactions = comment.Reactions.SumReactions(),
                HasPicture = !string.IsNullOrEmpty(comment.Picture),
            };

        public SubComment CreateSubComment(int subCommentId, int mainCommentId)
            => new SubComment
            {
                MainCommentId = mainCommentId,
                CommentId = subCommentId
            };
    }
}
