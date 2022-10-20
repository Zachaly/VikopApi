namespace VikopApi.Application.Models.Requests
{
    public class AddPostRequest : CommentRequest
    {
        public IEnumerable<string> Tags { get; set; }
    }
}
