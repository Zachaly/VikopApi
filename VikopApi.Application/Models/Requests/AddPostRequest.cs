namespace VikopApi.Application.Models.Requests
{
    public class AddPostRequest : AddCommentRequest
    {
        public IEnumerable<string> Tags { get; set; }
    }
}
