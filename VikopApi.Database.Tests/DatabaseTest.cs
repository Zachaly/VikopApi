﻿using Microsoft.EntityFrameworkCore;

namespace VikopApi.Database.Tests
{
    public class DatabaseTest : IDisposable
    {
        protected readonly AppDbContext _dbContext;

        public DatabaseTest()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>();
            options.UseInMemoryDatabase(Guid.NewGuid().ToString());
            _dbContext = new AppDbContext(options.Options);

            UsersSetup();
            FindingSetup();
            CommentsSetup();
            SubCommentsSetup();

            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
        }

        private void UsersSetup()
        {
            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "user1",
                    Id = "1",
                    Email = "user1@email.com",
                    ProfilePicture = "user1.jpg",
                    Created = new DateTime(2021, 1, 1),
                },
                new ApplicationUser
                {
                    UserName = "user2",
                    Id = "2",
                    Email = "user2@email.com",
                    ProfilePicture = "user2.jpg",
                    Created = new DateTime(2022, 2, 2),
                },
                new ApplicationUser
                {
                    UserName = "user3",
                    Id = "3",
                    Email = "user3@email.com",
                    ProfilePicture = "user3.jpg",
                    Created = new DateTime(2022, 3, 3),
                }
            };

            _dbContext.Users.AddRange(users);
        }

        private void FindingSetup()
        {
            var findings = new List<Finding>
            {
                new Finding
                {
                    Id = 1,
                    Title = "finding1",
                    Description = "description1",
                    Created = new DateTime(2005, 4, 2),
                    CreatorId = "1",
                    Link = "https://link1.com",
                    Picture = "finding1.jpg"
                },
                new Finding
                {
                    Id = 2,
                    Title = "finding2",
                    Description = "description2",
                    Created = new DateTime(2022, 4, 2),
                    CreatorId = "2",
                    Link = "https://link2.com",
                    Picture = "finding2.jpg"
                },
                new Finding
                {
                    Id = 3,
                    Title = "finding3",
                    Description = "description3",
                    Created = new DateTime(2020, 3, 30),
                    CreatorId = "2",
                    Link = "https://link3.com",
                    Picture = "finding3.jpg"
                },
            };

            var reactions = new List<FindingReaction>
            {
                new FindingReaction { FindingId = 1, UserId = "1", Reaction = Reaction.Positive },
                new FindingReaction { FindingId = 1, UserId = "2", Reaction = Reaction.Positive },
                new FindingReaction { FindingId = 1, UserId = "3", Reaction = Reaction.Negative },
                new FindingReaction { FindingId = 2, UserId = "2", Reaction = Reaction.Negative },
                new FindingReaction { FindingId = 2, UserId = "3", Reaction = Reaction.Negative },
                new FindingReaction { FindingId = 3, UserId = "3", Reaction = Reaction.Positive },
                new FindingReaction { FindingId = 3, UserId = "1", Reaction = Reaction.Positive },
            };

            _dbContext.Findings.AddRange(findings);
            _dbContext.FindingReactions.AddRange(reactions);
        }

        private void CommentsSetup()
        {
            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 1,
                    Content = "comment1",
                    Created = DateTime.Now,
                    CreatorId = "1"
                },
                new Comment
                {
                    Id = 2,
                    Content = "comment2",
                    Created = DateTime.Now,
                    CreatorId = "1"
                },
                new Comment
                {
                    Id = 3,
                    Content = "comment3",
                    Created = DateTime.Now,
                    CreatorId = "2"
                },
                new Comment
                {
                    Id = 4,
                    Content = "comment4",
                    Created = DateTime.Now,
                    CreatorId = "3"
                },
                new Comment
                {
                    Id = 5,
                    Content = "comment5",
                    Created = DateTime.Now,
                    CreatorId = "3"
                }
            };

            var findingComments = new List<FindingComment>
            {
                new FindingComment { FindingId = 1, CommentId = 1},
                new FindingComment { FindingId = 2, CommentId = 2},
                new FindingComment { FindingId = 3, CommentId = 3},
                new FindingComment { FindingId = 3, CommentId = 4},
                new FindingComment { FindingId = 3, CommentId = 5},
            };

            var reactions = new List<CommentReaction>
            {
                new CommentReaction { CommentId = 1, UserId = "2", Reaction = Reaction.Negative },
                new CommentReaction { CommentId = 2, UserId = "3", Reaction = Reaction.Negative },
                new CommentReaction { CommentId = 3, UserId = "2", Reaction = Reaction.Positive },
                new CommentReaction { CommentId = 4, UserId = "3", Reaction = Reaction.Positive },
                new CommentReaction { CommentId = 4, UserId = "2", Reaction = Reaction.Positive },
                new CommentReaction { CommentId = 5, UserId = "1", Reaction = Reaction.Positive },
                new CommentReaction { CommentId = 5, UserId = "2", Reaction = Reaction.Negative },
                new CommentReaction { CommentId = 5, UserId = "3", Reaction = Reaction.Negative },
            };

            _dbContext.AddRange(comments);
            _dbContext.AddRange(findingComments);
            _dbContext.AddRange(reactions);
        }

        private void SubCommentsSetup()
        {
            var comments = new List<Comment>
            {
                new Comment
                {
                    Id = 11,
                    Content = "subcomment1",
                    Created = new DateTime(2022, 2, 2),
                    CreatorId = "1",
                },
                new Comment
                {
                    Id = 12,
                    Content = "subcomment2",
                    Created = new DateTime(2021, 2, 2),
                    CreatorId = "1",
                },
                new Comment
                {
                    Id = 13,
                    Content = "subcomment3",
                    Created = new DateTime(2021, 2, 2),
                    CreatorId = "2",
                },
                new Comment
                {
                    Id = 14,
                    Content = "subcomment4",
                    Created = new DateTime(2021, 2, 2),
                    CreatorId = "3",
                },
            };

            var subcomments = new List<SubComment>
            {
                new SubComment
                {
                    CommentId = 11,
                    MainCommentId = 1
                },
                new SubComment
                {
                    CommentId = 12,
                    MainCommentId = 1
                },
                new SubComment
                {
                    CommentId = 13,
                    MainCommentId = 2
                },
                new SubComment
                {
                    CommentId = 14,
                    MainCommentId = 3
                },
            };

            _dbContext.Comments.AddRange(comments);
            _dbContext.SubComments.AddRange(subcomments);
        }
    }
}
