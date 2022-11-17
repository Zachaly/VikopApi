using VikopApi.Application.Comments.Abstractions;
using VikopApi.Application.Models.Comment;
using VikopApi.Application.Models.Comment.Requests;

namespace VikopApi.Application.Comments
{
    [Implementation(typeof(ICommentService))]
    public class CommentService : ICommentService
    {
        private readonly ICommentManager _commentManager;
        private readonly ICommentFactory _commentFactory;

        public CommentService(ICommentManager commentManager, ICommentFactory commentFactory)
        {
            _commentManager = commentManager;
            _commentFactory = commentFactory;
        }

        public async Task<CommentModel> AddFindingComment(AddFindingCommentRequest request)
        {
            var comment = _commentFactory.Create(request);
            await _commentManager.AddComment(comment);

            await _commentManager.AddFindingComment(comment.Id, request.FindingId);

            return _commentManager.GetCommentById(comment.Id, comment => _commentFactory.CreateModel(comment));
        }

        public async Task<CommentModel> AddSubcomment(AddSubcommentRequest request)
        {
            var comment = _commentFactory.Create(request);
            await _commentManager.AddComment(comment);

            var subcomment = _commentFactory.CreateSubComment(comment.Id, request.MainCommentId);

            await _commentManager.AddSubComment(subcomment);

            return _commentManager.GetCommentById(comment.Id, comment => _commentFactory.CreateModel(comment));
        }

        public CommentModel GetCommentById(int id)
            => _commentManager.GetCommentById(id, comment => _commentFactory.CreateModel(comment));

        public IEnumerable<CommentModel> GetSubcomments(int commentId)
            => _commentManager.GetSubComments(commentId, comment => _commentFactory.CreateModel(comment.Comment));
    }
}
