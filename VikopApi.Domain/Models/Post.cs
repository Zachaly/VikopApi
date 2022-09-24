namespace VikopApi.Domain.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
