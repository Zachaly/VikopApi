using VikopApi.Domain.Enums;

namespace VikopApi.Application.Comments
{
    [Service]
    public class ChangeReaction
    {
        private readonly ICommentManager _commentManager;

        public ChangeReaction(ICommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        public async Task<bool> Execute(Request request)
        {
            var reaction = new CommentReaction
            {
                CommentId = request.CommentId,
                UserId = request.UserId,
                Reaction = request.Reaction
            };

            return await _commentManager.ChangeReaction(reaction);
        }

        public class Request
        {
            public string UserId { get; set; }
            public int CommentId { get; set; }
            public Reaction Reaction { get; set; }
        }
    }
}
