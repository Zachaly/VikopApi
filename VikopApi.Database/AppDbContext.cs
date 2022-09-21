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
        public DbSet<FindingReaction> FindingReactions { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }
        public DbSet<SubComment> SubComments { get; set; }

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

            builder.Entity<FindingReaction>().HasKey(reaction => new { reaction.FindingId, reaction.UserId });
            builder.Entity<FindingReaction>()
                .HasOne(reaction => reaction.Finding)
                .WithMany(finding => finding.Reactions)
                .HasForeignKey(reaction => reaction.FindingId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<FindingReaction>()
                .HasOne(reaction => reaction.User)
                .WithMany(user => user.FindingReactions)
                .HasForeignKey(reaction => reaction.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CommentReaction>().HasKey(reaction => new { reaction.CommentId, reaction.UserId });
            builder.Entity<CommentReaction>()
                .HasOne(reaction => reaction.Comment)
                .WithMany(comment => comment.Reactions)
                .HasForeignKey(reaction => reaction.CommentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CommentReaction>()
                .HasOne(reaction => reaction.User)
                .WithMany(user => user.CommentReactions)
                .HasForeignKey(reaction => reaction.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<SubComment>().HasKey(comment => new { comment.CommentId, comment.MainCommentId });
            builder.Entity<SubComment>()
                .HasOne(comment => comment.MainComment)
                .WithMany(comment => comment.SubComments)
                .HasForeignKey(comment => comment.MainCommentId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}