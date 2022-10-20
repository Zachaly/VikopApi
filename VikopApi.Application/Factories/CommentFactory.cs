using VikopApi.Application.Factories.Abstractions;
using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.Factories
{
    public class CommentFactory : ICommentFactory
    {
        public Comment Create(CommentRequest commentRequest)
            => new Comment
            {
                Content = commentRequest.Content,
                CreatorId = commentRequest.CreatorId,
                Created = DateTime.Now,
                Picture = commentRequest.Picture
            }; 
    }
}
