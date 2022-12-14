using VikopApi.Domain.Enums;
using VikopApi.Domain.Models.Abstractions;

namespace VikopApi.Domain.Models
{
    public class FindingReaction : IReaction
    {
        public int FindingId { get; set; }
        public Finding Finding { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Reaction Reaction { get; set; }
    }
}
