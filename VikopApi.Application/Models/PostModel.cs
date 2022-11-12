
namespace VikopApi.Application.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public CommentModel Content { get; set; }
        public IEnumerable<Tag> TagList { get; set; }
    }
}
