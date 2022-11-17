using VikopApi.Application.Models.Comment;

namespace VikopApi.Application.Models.Finding
{
    public class FindingModel
    {
        public FindingListItemModel Finding { get; set; }
        public IEnumerable<CommentModel> Comments { get; set; }
    }
}
