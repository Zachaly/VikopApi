using VikopApi.Application.Comments.Abstractions;
using VikopApi.Application.Findings.Abstractions;
using VikopApi.Application.Models.Finding;
using VikopApi.Application.Models.Finding.Requests;

namespace VikopApi.Application.Findings
{
    [Implementation(typeof(IFindingFactory))]
    public class FindingFactory : IFindingFactory
    {
        private readonly ICommentFactory _commentFactory;

        public FindingFactory(ICommentFactory commentFactory)
        {
            _commentFactory = commentFactory;
        }

        public Finding Create(AddFindingRequest request)
            => new Finding
            {
                Created = DateTime.Now,
                CreatorId = request.CreatorId,
                Description = request.Description,
                Link = request.Link,
                Picture = request.Picture,
                Title = request.Title,
            };

        public FindingListItemModel CreateListItem(Finding finding)
            => new FindingListItemModel
            {
                CommentCount = finding.Comments.Count(),
                Created = finding.Created.GetTime(),
                CreatorId = finding.CreatorId,
                CreatorName = finding.Creator.UserName,
                CreatorRank = finding.Creator.Rank,
                Description = finding.Description,
                Id = finding.Id,
                Link = finding.Link,
                Reactions = finding.Reactions.SumReactions(),
                TagList = finding.Tags.Select(tag => tag.Tag),
                Title = finding.Title
            };

        public FindingModel CreateModel(Finding finding)
            => new FindingModel
            {
                Finding = CreateListItem(finding),
                Comments = finding.Comments.Select(comment => _commentFactory.CreateModel(comment.Comment))
            };
    }
}
