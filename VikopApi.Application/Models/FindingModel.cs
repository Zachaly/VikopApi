namespace VikopApi.Application.Models
{
    public class FindingModel
    {
        public FindingListItemModel Finding { get; set; }
        public IEnumerable<CommentModel> Comments { get; set; }
    }
}
