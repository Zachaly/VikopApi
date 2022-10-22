using VikopApi.Domain.Enums;

namespace VikopApi.Application.Models
{
    public class FindingListItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public Rank CreatorRank { get; set; }
        public string Link { get; set; }
        public int CommentCount { get; set; }
        public string Created { get; set; }
        public int Reactions { get; set; }
        public IEnumerable<Tag> TagList { get; set; }

        public FindingListItemModel()
        {

        }

        public FindingListItemModel(Finding finding)
        {
            CreatorId = finding.CreatorId;
            CreatorName = finding.Creator.UserName;
            CreatorRank = finding.Creator.Rank;
            Description = finding.Description;
            Id = finding.Id;
            Link = finding.Link;
            Title = finding.Title;
            CommentCount = finding.Comments.Count;
            Created = finding.Created.GetTime();
            Reactions = finding.Reactions.SumReactions();
            TagList = finding.Tags.Select(tag => tag.Tag);
        }
    }
}
