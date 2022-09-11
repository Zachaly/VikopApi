
namespace VikopApi.Domain.Models
{
    public class Finding
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Picture { get; set; }
        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; }
        public DateTime Created { get; set; }
        public ICollection<FindingComment> Comments { get; set; }
    }
}
