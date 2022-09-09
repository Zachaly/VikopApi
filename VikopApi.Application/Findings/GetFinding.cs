
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
                Title = finding.Title
            });

        public class Response
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string CreatorName { get; set; }
            public string Link { get; set; }
        }
    }
}
