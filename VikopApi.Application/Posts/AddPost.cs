using VikopApi.Application.HelperModels;

namespace VikopApi.Application.Posts
{
    [Service]
    public class AddPost
    {
        private readonly ICommentManager _commentManager;
        private readonly IPostManager _postManager;

        public AddPost(ICommentManager commentManager, IPostManager postManager)
        {
            _commentManager = commentManager;
            _postManager = postManager;
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

            var post = new Post
            {
                CommentId = comment.Id
            };

            await _postManager.AddPost(post);

            return _commentManager.GetCommentById(comment.Id, comment => new CommentModel(comment));
        }

        public class Request : CommentRequest { }
    }
}
