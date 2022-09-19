
namespace VikopApi.Application.Findings
{
    [Service]
    public class GetFindings
    {
        private readonly IFindingManager _findingManager;

        public GetFindings(IFindingManager findingManager)
        {
            _findingManager = findingManager;
        }

        public IEnumerable<FindingModel> Execute()
            => _findingManager.GetFindings( finding => new FindingModel
            {
                CreatorName = finding.Creator.UserName,
                Description = finding.Description,
                Id = finding.Id,
                Link = finding.Link,
                Title = finding.Title,
                CommentCount = finding.Comments.Count,
                Created = finding.Created,
                Reactions = finding.Reactions.Sum(reaction => (int)reaction.Reaction)
            }).OrderByDescending(finding => finding.Id);

        public class FindingModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string CreatorName { get; set; }
            public string Link { get; set; }
            public int CommentCount { get; set; }
            public DateTime Created { get; set; }
            public int Reactions { get; set; }
        }
    }
}
