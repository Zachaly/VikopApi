using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VikopApi.Domain.Models;

namespace VikopApi.Database
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Finding> Findings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FindingComment> FindingComments { get; set; }

        public AppDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<FindingComment>().HasKey(comment => new { comment.FindingId, comment.CommentId });
            builder.Entity<FindingComment>()
                .HasOne(comment => comment.Finding)
                .WithMany(finding => finding.Comments)
                .HasForeignKey(comment => comment.FindingId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}