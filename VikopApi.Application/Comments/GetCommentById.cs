
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

        public Response Execute(int id) 
            => _commentManager.GetCommentById(id, comment => new Response
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    Created = comment.Created,
                    CreatorId = comment.CreatorId,
                    CreatorName = comment.Creator.UserName
                });
        public class Response
        {
            public int Id { get; set; }
            public string CreatorId { get; set; }
            public string CreatorName { get; set; }
            public string Content { get; set; }
            public DateTime Created { get; set; }
        }
    }
}
