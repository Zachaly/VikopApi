using VikopApi.Application.Models.Requests;
using VikopApi.Application.Tags.Abtractions;

namespace VikopApi.Application.Findings
{
    [Service]
    public class AddFinding
    {
        private readonly IFindingManager _findingManager;
        private readonly ITagService _tagService;

        public AddFinding(IFindingManager findingManager, ITagFactory tagFactory, ITagService tagService)
        {
            _findingManager = findingManager;
            _tagService = tagService;
        }

        public async Task Execute(AddFindingRequest request)
        {
            var finding = new Finding
            {
                CreatorId = request.CreatorId,
                Title = request.Title,
                Description = request.Description,
                Link = request.Link,
                Picture = request.Picture,
                Created = DateTime.Now,
            };

            await _findingManager.AddFinding(finding);

            await _tagService.CreateFinding(request.TagList, finding.Id);
        }

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
