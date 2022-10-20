using VikopApi.Domain.Enums;
using VikopApi.Domain.Models.Abstractions;

namespace VikopApi.Domain.Models
{
    public class CommentReaction : IReaction
    {

        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Reaction Reaction { get; set; }
    }
}
