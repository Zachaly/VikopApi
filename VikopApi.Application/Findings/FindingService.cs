using VikopApi.Application.Findings.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Tags.Abtractions;
using VikopApi.Domain.Enums;

namespace VikopApi.Application.Findings
{
    [Implementation(typeof(IFindingService))]
    public class FindingService : IFindingService
    {
        private readonly IFindingFactory _findingFactory;
        private readonly IFindingManager _findingManager;
        private readonly ITagService _tagService;

        public FindingService(IFindingFactory findingFactory, IFindingManager findingManager, ITagService tagService)
        {
            _findingFactory = findingFactory;
            _findingManager = findingManager;
            _tagService = tagService;
        }

        public async Task<int> AddFinding(AddFindingRequest request)
        {
            var finding = _findingFactory.Create(request);

            await _findingManager.AddFinding(finding);

            await _tagService.CreateFinding(request.TagList, finding.Id);

            return finding.Id;
        }

        public FindingModel GetFindingById(int id)
            => _findingManager.GetFindingById(id, finding => _findingFactory.CreateModel(finding));

        public IEnumerable<FindingListItemModel> GetFindings(SortingType? sortingType, int? pageIndex, int? pageSize)
        {
            Func<Finding, FindingListItemModel> selector = finding => _findingFactory.CreateListItem(finding);

            var page = pageIndex ?? 0;
            var size = pageSize ?? 10;

            if (sortingType.HasValue)
            {
                if(sortingType.Value == SortingType.New)
                {
                    return _findingManager.GetNewFindings(page, size, selector);
                }
                else if(sortingType.Value == SortingType.Top)
                {
                    return _findingManager.GetTopFindings(page, size, selector);
                }
            } 
            
            return _findingManager.GetAllFindings(page, size, selector);
        }

        public int GetPageCount(int pageSize)
            => _findingManager.GetPageCount(pageSize);

        public IEnumerable<FindingListItemModel> Search(SearchFindingsRequest request)
        {
            var conditions = new List<Func<Finding, bool>>();

            if(request.SearchTitle.GetValueOrDefault())
                conditions.Add(finding => finding.Title.ToLower().Contains(request.Text.ToLower()));
            if (request.SearchCreator.GetValueOrDefault())
                conditions.Add(finding => finding.Creator.UserName.ToLower().Contains(request.Text.ToLower()));
            if (request.SearchTag.GetValueOrDefault())
                conditions.Add(finding => finding.Tags.Any(tag => tag.Tag.Name.ToLower().Contains(request.Text.ToLower())));

            return _findingManager.SearchFindings(request.PageIndex ?? 0, request.PageSize ?? 10, conditions, finding => _findingFactory.CreateListItem(finding));
        }
    }
}
