
namespace VikopApi.Domain.Models
{
    public class FindingComment
    {
        public int FindingId { get; set; }
        public int CommentId { get; set; }
        public Finding Finding { get; set; }
        public Comment Comment { get; set; }
    }
}
