using VikopApi.Application.HelperModels;

namespace VikopApi.Application.Comments
{
    [Service]
    public class GetCommentById
    {
        private readonly ICommentManager _commentManager;

        public GetCommentById(ICommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        public CommentModel Execute(int id) 
            => _commentManager.GetCommentById(id, comment => new CommentModel(comment));
    }
}
