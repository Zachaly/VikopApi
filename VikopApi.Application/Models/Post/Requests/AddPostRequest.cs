using VikopApi.Application.Models.Comment.Requests;

namespace VikopApi.Application.Models.Post.Requests
{
    public class AddPostRequest : AddCommentRequest
    {
        public IEnumerable<string> Tags { get; set; }
    }
}
