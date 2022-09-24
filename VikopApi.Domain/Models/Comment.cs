namespace VikopApi.Domain.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public ICollection<CommentReaction> Reactions { get; set; }
        public ICollection<SubComment> SubComments { get; set; }
    }
}
