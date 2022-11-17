namespace VikopApi.Application.Models.Comment.Requests
{
    public class AddSubcommentRequest : AddCommentRequest
    {
        public int MainCommentId { get; set; }
    }
}
