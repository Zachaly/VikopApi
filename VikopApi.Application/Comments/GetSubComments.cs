using VikopApi.Application.Models;

namespace VikopApi.Application.Comments
{
    [Service]
    public class GetSubComments
    {
        private readonly ICommentManager _commentManager;

        public GetSubComments(ICommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        public IEnumerable<CommentModel> Execute(int commentId)
            => _commentManager.GetSubComments(commentId, subcomment => new CommentModel(subcomment.Comment));
    }
}
