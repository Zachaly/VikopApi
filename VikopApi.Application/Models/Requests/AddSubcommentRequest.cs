namespace VikopApi.Application.Models.Requests
{
    public class AddSubcommentRequest : AddCommentRequest
    {
        public int MainCommentId { get; set; }
    }
}
