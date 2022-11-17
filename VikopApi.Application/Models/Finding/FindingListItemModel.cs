using VikopApi.Domain.Enums;

namespace VikopApi.Application.Models.Finding
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
    }
}
