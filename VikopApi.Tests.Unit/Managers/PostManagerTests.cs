using VikopApi.Database;
using VikopApi.Domain.Enums;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Managers
{
    [TestFixture]
    public class PostManagerTests
    {
        [Test]
        public async Task AddPost()
        {
            var dbContext = Extensions.GetAppDbContext();
            var manager = new PostManager(dbContext);
            var post = new Post
            {
                Id = 1
            };

            var res = await manager.AddPost(post);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(dbContext.Posts.Contains(post));
                Assert.That(dbContext.Posts.Count(), Is.EqualTo(1));
            });
        }

        [Test]
        [TestCase(0, 2)]
        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(3, 1)]
        [TestCase(4, 1)]
        public void GetPosts(int index, int size)
        {
            var posts = new List<Post>
            {
                new Post { Id = 1, Comment = new Comment{ Id = 1 } },
                new Post { Id = 2, Comment = new Comment{ Id = 2 }  },
                new Post { Id = 3, Comment = new Comment{ Id = 3 }  },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(posts);
            var manager = new PostManager(dbContext);

            var res = manager.GetPosts(index, size, x => x).ToList();

            Assert.That(res, Is.EquivalentTo(posts.Skip(index * size).Take(size)));
        }

        [Test]
        public void GetTopPosts_ReactionsCount()
        {
            var posts = new List<Post>
            {
                new Post 
                { 
                    Id = 1,
                    Comment = new Comment
                    { 
                        Id = 1,
                        Reactions = new List<CommentReaction>
                        {
                            new CommentReaction { Reaction = Reaction.Negative, CommentId = 1, UserId = "id1" },
                            new CommentReaction { Reaction = Reaction.Negative, CommentId = 1, UserId = "id2" },
                            new CommentReaction { Reaction = Reaction.Negative, CommentId = 1, UserId = "id3" },
                        }
                    }
                },
                new Post 
                { 
                    Id = 2,
                    Comment = new Comment
                    { 
                        Id = 2,
                        Reactions = new List<CommentReaction>
                        {
                            new CommentReaction { Reaction = Reaction.Negative, CommentId = 2, UserId = "id1" },
                            new CommentReaction { Reaction = Reaction.Positive, CommentId = 2, UserId = "id2" },
                            new CommentReaction { Reaction = Reaction.Negative, CommentId = 2, UserId = "id3" },
                        }
                    }
                },
                new Post 
                { 
                    Id = 3,
                    Comment = new Comment
                    { 
                        Id = 3,
                        Reactions = new List<CommentReaction>
                        {
                            new CommentReaction { Reaction = Reaction.Positive, CommentId = 3, UserId = "id1" },
                            new CommentReaction { Reaction = Reaction.Positive, CommentId = 3, UserId = "id2" },
                            new CommentReaction { Reaction = Reaction.Positive, CommentId = 3, UserId = "id3" },
                        }
                    }
                },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(posts);
            var manager = new PostManager(dbContext);

            var res = manager.GetTopPosts(0, 3, x => x);

            Assert.That(res, Is.EquivalentTo(posts.OrderByDescending(x => x.Comment.Reactions.Sum(y => (int)y.Reaction))));
        }

        [Test]
        public void GetTopPosts_SubCommentsCount()
        {
            var posts = new List<Post>
            {
                new Post
                {
                    Id = 1,
                    Comment = new Comment
                    {
                        Id = 1,
                        SubComments = new List<SubComment>
                        {
                            new SubComment
                            {
                                MainCommentId = 1,
                                CommentId = 10,
                                Comment = new Comment
                                {
                                    Id = 10,
                                    Reactions = new List<CommentReaction>
                                    {
                                        new CommentReaction { CommentId = 10, UserId = "id1", Reaction = Reaction.Negative },
                                        new CommentReaction { CommentId = 10, UserId = "id2", Reaction = Reaction.Negative },
                                        new CommentReaction { CommentId = 10, UserId = "id3", Reaction = Reaction.Negative },
                                    }
                                }
                            },
                            new SubComment
                            {
                                MainCommentId = 1,
                                CommentId = 11,
                                Comment = new Comment
                                {
                                    Id = 11,
                                    Reactions = new List<CommentReaction>
                                    {
                                        new CommentReaction { CommentId = 11, UserId = "id1", Reaction = Reaction.Negative },
                                        new CommentReaction { CommentId = 11, UserId = "id2", Reaction = Reaction.Negative },
                                        new CommentReaction { CommentId = 11, UserId = "id3", Reaction = Reaction.Negative },
                                    }
                                }
                            },
                            new SubComment
                            {
                                MainCommentId = 1,
                                CommentId = 12,
                                Comment = new Comment
                                {
                                    Id = 12,
                                    Reactions = new List<CommentReaction>
                                    {
                                        new CommentReaction { CommentId = 12, UserId = "id1", Reaction = Reaction.Negative },
                                        new CommentReaction { CommentId = 12, UserId = "id2", Reaction = Reaction.Negative },
                                        new CommentReaction { CommentId = 12, UserId = "id3", Reaction = Reaction.Negative },
                                    }
                                }
                            },
                        }
                    }
                },
                new Post
                {
                    Id = 2,
                    Comment = new Comment
                    {
                        Id = 2,
                        SubComments = new List<SubComment>
                        {
                            new SubComment
                            {
                                MainCommentId = 2,
                                CommentId = 13,
                                Comment = new Comment
                                {
                                    Id = 13,
                                    Reactions = new List<CommentReaction>
                                    {
                                        new CommentReaction { CommentId = 13, UserId = "id1", Reaction = Reaction.Positive },
                                        new CommentReaction { CommentId = 13, UserId = "id2", Reaction = Reaction.Negative },
                                        new CommentReaction { CommentId = 13, UserId = "id3", Reaction = Reaction.Negative },
                                    }
                                }
                            },
                            new SubComment
                            {
                                MainCommentId = 2,
                                CommentId = 14,
                                Comment = new Comment
                                {
                                    Id = 14,
                                    Reactions = new List<CommentReaction>
                                    {
                                        new CommentReaction { CommentId = 14, UserId = "id1", Reaction = Reaction.Positive },
                                        new CommentReaction { CommentId = 14, UserId = "id2", Reaction = Reaction.Negative },
                                        new CommentReaction { CommentId = 14, UserId = "id3", Reaction = Reaction.Negative },
                                    }
                                }
                            },
                            new SubComment
                            {
                                MainCommentId = 1,
                                CommentId = 15,
                                Comment = new Comment
                                {
                                    Id = 15,
                                    Reactions = new List<CommentReaction>
                                    {
                                        new CommentReaction { CommentId = 15, UserId = "id1", Reaction = Reaction.Negative },
                                        new CommentReaction { CommentId = 15, UserId = "id2", Reaction = Reaction.Positive },
                                        new CommentReaction { CommentId = 15, UserId = "id3", Reaction = Reaction.Negative },
                                    }
                                }
                            },
                        }
                    }
                },
                new Post
                {
                    Id = 3,
                    Comment = new Comment
                    {
                        Id = 3,
                        SubComments = new List<SubComment>
                        {
                            new SubComment
                            {
                                MainCommentId = 3,
                                CommentId = 23,
                                Comment = new Comment
                                {
                                    Id = 23,
                                    Reactions = new List<CommentReaction>
                                    {
                                        new CommentReaction { CommentId = 23, UserId = "id1", Reaction = Reaction.Positive },
                                        new CommentReaction { CommentId = 23, UserId = "id2", Reaction = Reaction.Positive },
                                        new CommentReaction { CommentId = 23, UserId = "id3", Reaction = Reaction.Negative },
                                    }
                                }
                            },
                            new SubComment
                            {
                                MainCommentId = 3,
                                CommentId = 24,
                                Comment = new Comment
                                {
                                    Id = 24,
                                    Reactions = new List<CommentReaction>
                                    {
                                        new CommentReaction { CommentId = 24, UserId = "id1", Reaction = Reaction.Positive },
                                        new CommentReaction { CommentId = 24, UserId = "id2", Reaction = Reaction.Positive },
                                        new CommentReaction { CommentId = 24, UserId = "id3", Reaction = Reaction.Negative },
                                    }
                                }
                            },
                            new SubComment
                            {
                                MainCommentId = 3,
                                CommentId = 25,
                                Comment = new Comment
                                {
                                    Id = 25,
                                    Reactions = new List<CommentReaction>
                                    {
                                        new CommentReaction { CommentId = 25, UserId = "id1", Reaction = Reaction.Positive },
                                        new CommentReaction { CommentId = 25, UserId = "id2", Reaction = Reaction.Positive },
                                        new CommentReaction { CommentId = 25, UserId = "id3", Reaction = Reaction.Negative },
                                    }
                                }
                            },
                        }
                    }
                },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(posts);
            var manager = new PostManager(dbContext);

            var res = manager.GetTopPosts(0, 3, x => x);

            var orderedPosts = posts.OrderByDescending(
                x => x.Comment.SubComments.Count() 
                + x.Comment.SubComments
                    .Select(y => y.Comment.Reactions.Sum(z => (int)z.Reaction)).Sum());

            Assert.That(res, Is.EquivalentTo(orderedPosts));
        }

        [Test]
        public void GetnewPosts()
        {
            var posts = new List<Post>
            {
                new Post { Id = 1, Comment = new Comment{ Id = 1, Created = DateTime.Now.AddDays(-1) } },
                new Post { Id = 2, Comment = new Comment{ Id = 2, Created = DateTime.Now } },
                new Post { Id = 3, Comment = new Comment{ Id = 3, Created = DateTime.Now.AddDays(-2137) }  },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(posts);
            var manager = new PostManager(dbContext);

            var res = manager.GetNewPosts(0, 3, x => x).ToList();

            Assert.That(res.First(), Is.EqualTo(posts.OrderByDescending(x => x.Comment.Created).First()));
        }

        [Test]
        [TestCase(2, 5)]
        [TestCase(3, 3)]
        [TestCase(1, 9)]
        [TestCase(8, 2)]
        [TestCase(5, 2)]
        public void GetPageCount(int size, int expectedCount)
        {
            var posts = new List<Post>
            {
                new Post { Id = 1 },
                new Post { Id = 2 },
                new Post { Id = 3 },
                new Post { Id = 4 },
                new Post { Id = 5 },
                new Post { Id = 6 },
                new Post { Id = 7 },
                new Post { Id = 8 },
                new Post { Id = 9 },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(posts);
            var manager = new PostManager(dbContext);

            var res = manager.GetPageCount(size);

            Assert.That(res, Is.EqualTo(expectedCount));
        }
    }
}
