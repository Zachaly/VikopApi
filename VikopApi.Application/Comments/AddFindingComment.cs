using VikopApi.Application.Factories.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.Comments
{
    [Service]
    public class AddFindingComment
    {
        private readonly ICommentManager _commentManager;
        private readonly ICommentFactory _commentFactory;

        public AddFindingComment(ICommentManager commentManager, ICommentFactory commentFactory)
        {
            _commentManager = commentManager;
            _commentFactory = commentFactory;
        }

        public async Task<CommentModel> Execute(Request request)
        {
            var comment = _commentFactory.Create(request);

            var res = await _commentManager.AddComment(comment);

            if (!res)
                return null;

            await _commentManager.AddFindingComment(comment.Id, request.FindingId);
            return _commentManager.GetCommentById(comment.Id, comment => new CommentModel(comment));
        }

        public class Request : CommentRequest
        {
            public int FindingId { get; set; }
        }
    }
}
