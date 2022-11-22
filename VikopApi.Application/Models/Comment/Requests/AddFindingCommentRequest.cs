namespace VikopApi.Application.Models.Comment.Requests
{
    public class AddFindingCommentRequest : AddCommentRequest
    {
        public int FindingId { get; set; }
    }
}
