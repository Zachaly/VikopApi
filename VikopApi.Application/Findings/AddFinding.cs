
namespace VikopApi.Application.Findings
{
    [Service]
    public class AddFinding
    {
        private readonly IFindingManager _findingManager;

        public AddFinding(IFindingManager findingManager)
        {
            _findingManager = findingManager;
        }

        public async Task<bool> Execute(Request request)
            => await _findingManager.AddFinding(new Finding
            {
                CreatorId = request.CreatorId,
                Title = request.Title,
                Description = request.Description,
                Link = request.Link,
                Picture = request.Picture
            });

        public class Request
        {
            public string Title { get; set; }
            public string CreatorId { get; set; }
            public string Description { get; set; }
            public string Link { get; set; }
            public string Picture { get; set; }
        }
    }
}
