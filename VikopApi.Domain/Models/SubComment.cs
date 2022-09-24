namespace VikopApi.Domain.Models
{
    public class SubComment
    {
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        public int MainCommentId { get; set; }
        public Comment MainComment { get; set; }
    }
}
