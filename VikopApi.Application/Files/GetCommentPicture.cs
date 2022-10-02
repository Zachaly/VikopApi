namespace VikopApi.Application.Files
{
    [Service]
    public class GetCommentPicture
    {
        private readonly ICommentManager _commentManager;

        public GetCommentPicture(ICommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        public string Execute(int id) => _commentManager.GetCommentById(id, comment => comment.Picture);
    }
}
