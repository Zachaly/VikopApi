using VikopApi.Domain.Models.Abstractions;

namespace VikopApi.Domain.Models
{
    public class PostTag : TagModel
    {
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
