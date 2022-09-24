namespace VikopApi.Application.Findings
{
    [Service]
    public class GetNewFindings
    {
        private readonly IFindingManager _findingManager;

        public GetNewFindings(IFindingManager findingManager)
        {
            _findingManager = findingManager;
        }

        public IEnumerable<FindingModel> Execute()
            => _findingManager.GetNewFindings(finding => new FindingModel
            {
                CreatorId = finding.CreatorId,
                CreatorName = finding.Creator.UserName,
                Description = finding.Description,
                Id = finding.Id,
                Link = finding.Link,
                Title = finding.Title,
                CommentCount = finding.Comments.Count,
                Created = finding.Created.GetTime(),
                Reactions = finding.Reactions.SumReactions()
            });

        public class FindingModel
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string CreatorId { get; set; }
            public string CreatorName { get; set; }
            public string Link { get; set; }
            public int CommentCount { get; set; }
            public string Created { get; set; }
            public int Reactions { get; set; }
        }
    }
}
