using VikopApi.Domain.Models.Abstractions;

namespace VikopApi.Domain.Models
{
    public class PostReport : Report
    {
        public int? PostId { get; set; }
        public Post? Post { get; set; }
    }
}
