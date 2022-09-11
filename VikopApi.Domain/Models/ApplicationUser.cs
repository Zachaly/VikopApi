using Microsoft.AspNetCore.Identity;
using VikopApi.Domain.Enums;

namespace VikopApi.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePicture { get; set; }
        public Rank Rank { get; set; }
        public ICollection<Finding> Findings { get; set; }
        public DateTime Created { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
