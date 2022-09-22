
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
            => _commentManager.GetSubComments(commentId, subcomment => new CommentModel
            {
                Id = subcomment.Comment.Id,
                Content = subcomment.Comment.Content,
                Created = subcomment.Comment.Created.GetTime(),
                CreatorId = subcomment.Comment.CreatorId,
                CreatorName = subcomment.Comment.Creator.UserName,
                Reactions = subcomment.Comment.Reactions.Sum(reaction => (int)reaction.Reaction)
            });

        public class CommentModel
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
