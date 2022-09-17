using VikopApi.Domain.Enums;

namespace VikopApi.Domain.Models
{
    public class FindingReaction
    {
        public int FindingId { get; set; }
        public Finding Finding { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Reaction Reaction { get; set; }
    }
}
