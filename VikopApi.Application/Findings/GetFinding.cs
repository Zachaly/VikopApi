using VikopApi.Application.Models;

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

        public FindingModel Execute(int id) 
            => _findingManager.GetFindingById(id, finding => new FindingModel
            {
                Finding = new FindingListItemModel(finding),
                Comments = finding.Comments.Select(comment => comment.Comment)
                    .Select(comment => new CommentModel(comment))
                    .OrderByDescending(comment => comment.Created),
            });
    }
}
