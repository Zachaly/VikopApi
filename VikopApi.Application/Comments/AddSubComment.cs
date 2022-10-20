using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.Comments
{
    [Service]
    public class AddSubComment
    {
        private readonly ICommentManager _commentManager;

        public AddSubComment(ICommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        public async Task<CommentModel> Execute(Request request)
        {
            var comment = new Comment
            {
                Content = request.Content,
                Created = DateTime.Now,
                CreatorId = request.CreatorId,
                Picture = request.Picture,
            };

            var res = await _commentManager.AddComment(comment);

            if (!res)
                return null;

            var subcomment = new SubComment
            {
                CommentId = comment.Id,
                MainCommentId = request.MainCommentId
            };

            await _commentManager.AddSubComment(subcomment);

            return _commentManager.GetCommentById(comment.Id, comment => new CommentModel(comment));
        }

        public class Request : CommentRequest
        {
            public int MainCommentId { get; set; }
        }
    }
}
