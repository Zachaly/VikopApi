namespace VikopApi.Application.Comments
{
    [Service]
    public class GetNewPosts
    {
        private readonly ICommentManager _commentManager;

        public GetNewPosts(ICommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        public IEnumerable<PostModel> Execute()
            => _commentManager.GetNewPosts(post => new PostModel
            {
                Id = post.Comment.Id,
                Content = post.Comment.Content,
                Created = post.Comment.Created.GetTime(),
                CreatorId = post.Comment.CreatorId,
                CreatorName = post.Comment.Creator.UserName,
                Reactions = post.Comment.Reactions.SumReactions()
            });

        public class PostModel
        {
            public int Id { get; set; }
            public string CreatorId { get; set; }
            public string CreatorName { get; set; }
            public string Content { get; set; }
            public string Created { get; set; }
            public int Reactions { get; set; }
        }
    }
}
