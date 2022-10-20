
namespace VikopApi.Application.Models
{
    public class PostModel
    {
        public CommentModel Content { get; set; }
        public IEnumerable<Tag> TagList { get; set; }
    }
}
