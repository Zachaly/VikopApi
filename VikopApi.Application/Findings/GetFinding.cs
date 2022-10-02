using VikopApi.Application.HelperModels;

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
                CreatorId = finding.CreatorId,
                CreatorName = finding.Creator.UserName,
                Description = finding.Description,
                Id = finding.Id,
                Link = finding.Link,
                Title = finding.Title,
                Created = finding.Created.GetTime(),
                Comments = finding.Comments.Select(comment => comment.Comment)
                    .Select(comment => new CommentModel(comment))
                    .OrderByDescending(comment => comment.Created),
                Reactions = finding.Reactions.SumReactions(),
                CommentCount = finding.Comments.Count()
            });

        public class Response
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string CreatorId { get; set; }
            public string CreatorName { get; set; }
            public string Link { get; set; }
            public string Created { get; set; }
            public IEnumerable<CommentModel> Comments { get; set; }
            public int Reactions { get; set; }
            public int CommentCount { get; set; }
        }
    }
}
