namespace VikopApi.Application.User
{
    [Service]
    public class GetUserFindings
    {
        private IApplicationUserManager _appUserManager;

        public GetUserFindings(IApplicationUserManager applicationUserManager)
        {
            _appUserManager = applicationUserManager;
        }

        public IEnumerable<FindingModel> Execute(string id)
            => _appUserManager.GetUserFindings(id, finding => new FindingModel
            {
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
            public string CreatorName { get; set; }
            public string Link { get; set; }
            public int CommentCount { get; set; }
            public string Created { get; set; }
            public int Reactions { get; set; }
        }
    }
}
