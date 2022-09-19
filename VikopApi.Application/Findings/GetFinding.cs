
namespace VikopApi.Application.Findings
{
    [Service]
    public class GetFinding
    {
        private readonly IFindingManager _findingManager;

        public GetFinding(IFindingManager findingManager)
        {
            _findingManager = findingManager;
        }

        public Response Execute(int id) 
            => _findingManager.GetFindingById(id, finding => new Response
            {
                CreatorName = finding.Creator.UserName,
                Description = finding.Description,
                Id = finding.Id,
                Link = finding.Link,
                Title = finding.Title,
                Created = finding.Created,
                Comments = finding.Comments.Select(comment => comment.Comment)
                .Select(comment => new CommentModel
                {
                    Content = comment.Content,
                    Created = comment.Created,
                    CreatorId = comment.CreatorId,
                    CreatorName = comment.Creator.UserName,
                    Id = comment.Id,
                    Reactions = comment.Reactions.Sum(reaction => (int)reaction.Reaction)
                }).OrderByDescending(comment => comment.Created),
                Reactions = finding.Reactions.Sum(reaction => (int)reaction.Reaction)
            });

        public class Response
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string CreatorName { get; set; }
            public string Link { get; set; }
            public DateTime Created { get; set; }
            public IEnumerable<CommentModel> Comments { get; set; }
            public int Reactions { get; set; }
        }

        public class CommentModel
        {
            public int Id { get; set; }
            public string CreatorId { get; set; }
            public string CreatorName { get; set; }
            public string Content { get; set; }
            public DateTime Created { get; set; }
            public int Reactions { get; set; }
        }
    }
}
