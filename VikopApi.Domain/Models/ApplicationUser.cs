using Microsoft.AspNetCore.Identity;
using VikopApi.Domain.Enums;

namespace VikopApi.Domain.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePicture { get; set; }
        public Rank Rank { get; set; }
    }
}
