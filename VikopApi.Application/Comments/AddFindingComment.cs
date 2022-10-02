using VikopApi.Application.HelperModels;

namespace VikopApi.Application.Comments
{
    [Service]
    public class AddFindingComment
    {
        private readonly ICommentManager _commentManager;

        public AddFindingComment(ICommentManager commentManager)
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
                Picture = request.Picture
            };

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
