using Moq;
using VikopApi.Application;
using VikopApi.Application.Comments.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Comment;
using VikopApi.Application.Models.Comment.Requests;
using VikopApi.Application.Models.Post;
using VikopApi.Application.Models.Post.Requests;
using VikopApi.Application.Posts;
using VikopApi.Application.Posts.Abstractions;
using VikopApi.Application.Tags.Abtractions;
using VikopApi.Domain.Enums;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Services
{
    [TestFixture]
    public class PostServiceTests
    {
        [Test]
        public async Task AddPost_Success()
        {
            var random = new Random();
            var posts = new List<Post>();
            var comments = new List<Comment>();

            var commentFactoryMock = new Mock<ICommentFactory>();
            commentFactoryMock.Setup(x => x.Create(It.IsAny<AddCommentRequest>()))
                .Returns((AddCommentRequest request) => new Comment 
                { 
                    Content = request.Content,
                    CreatorId = request.CreatorId,
                    Picture = request.Picture
                });

            var commentManagerMock = new Mock<ICommentManager>();
            commentManagerMock.Setup(x => x.AddComment(It.IsAny<Comment>()))
                .Callback((Comment comment) => 
                {
                    comment.Id = random.Next(1, 100);
                    comments.Add(comment);
                })
                .ReturnsAsync(true);

            commentManagerMock.Setup(x => x.GetCommentById(It.IsAny<int>(), It.IsAny<Func<Comment, Comment>>()))
                .Returns((int id, Func<Comment, Comment> selector) => comments.Where(x => x.Id == id).Select(selector).FirstOrDefault());

            var postFactoryMock = new Mock<IPostFactory>();
            postFactoryMock.Setup(x => x.Create(It.IsAny<Comment>()))
                .Returns((Comment comment) => new Post { Comment = comment, CommentId = comment.Id });

            postFactoryMock.Setup(x => x.CreateModel(It.IsAny<Comment>(), It.IsAny<IEnumerable<Tag>>()))
                .Returns((Comment comment, IEnumerable<Tag> tags) => new PostModel
                {
                    Content = new CommentModel
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        CreatorId = comment.CreatorId,
                        HasPicture = !string.IsNullOrEmpty(comment.Picture)
                    },
                    TagList = tags
                });

            var postManagerMock = new Mock<IPostManager>();
            postManagerMock.Setup(x => x.AddPost(It.IsAny<Post>()))
                .Callback((Post post) =>
                {
                    post.Id = random.Next(1, 100);
                    posts.Add(post);
                });

            var tagServiceMock = new Mock<ITagService>();
            tagServiceMock.Setup(x => x.CreatePost(It.IsAny<IEnumerable<string>>(), It.IsAny<int>()))
                .ReturnsAsync((IEnumerable<string> names, int id) => names.Select(x => new Tag { Name = x }));

            var postService = new PostService(postFactoryMock.Object, postManagerMock.Object, commentFactoryMock.Object,
                commentManagerMock.Object, tagServiceMock.Object);

            var request = new AddPostRequest
            {
                Content = "content",
                CreatorId = "id",
                Picture = "pic",
                Tags = new List<string> { "name1", "name2", "name3" }
            };

            var res = await postService.AddPost(request);

            Assert.Multiple(() =>
            {
                Assert.That(res.Content.Id, Is.EqualTo(comments.FirstOrDefault().Id));
                Assert.That(res.TagList.All(x => request.Tags.Any(y => y == x.Name)));
                Assert.That(res.TagList.Count(), Is.EqualTo(request.Tags.Count()));
                Assert.That(res.Content.HasPicture);
                Assert.That(res.Content.Content, Is.EqualTo(request.Content));
                Assert.That(res.Content.CreatorId, Is.EqualTo(request.CreatorId));
            });
        }

        [Test]
        public async Task AddPost_Fail()
        {
            var random = new Random();
            var posts = new List<Post>();
            var comments = new List<Comment>();

            var commentFactoryMock = new Mock<ICommentFactory>();
            commentFactoryMock.Setup(x => x.Create(It.IsAny<AddCommentRequest>()))
                .Returns((AddCommentRequest request) => new Comment
                {
                    Content = request.Content,
                    CreatorId = request.CreatorId,
                    Picture = request.Picture
                });

            var commentManagerMock = new Mock<ICommentManager>();
            commentManagerMock.Setup(x => x.AddComment(It.IsAny<Comment>()))
                .ReturnsAsync(false);

            var postFactoryMock = new Mock<IPostFactory>();

            var postManagerMock = new Mock<IPostManager>();

            var tagServiceMock = new Mock<ITagService>();

            var postService = new PostService(postFactoryMock.Object, postManagerMock.Object, commentFactoryMock.Object,
                commentManagerMock.Object, tagServiceMock.Object);

            var request = new AddPostRequest
            {
                Content = "content",
                CreatorId = "id",
                Picture = "pic",
                Tags = new List<string> { "name1", "name2", "name3" }
            };

            var res = await postService.AddPost(request);

            Assert.That(res, Is.Null);
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

            var commentFactoryMock = new Mock<ICommentFactory>();

            var commentManagerMock = new Mock<ICommentManager>();

            var postFactoryMock = new Mock<IPostFactory>();

            var postManagerMock = new Mock<IPostManager>();
            postManagerMock.Setup(x => x.GetPageCount(It.IsAny<int>()))
                .Returns((int x) => (int)Math.Ceiling(posts.Count / (decimal)x));

            var tagServiceMock = new Mock<ITagService>();

            var postService = new PostService(postFactoryMock.Object, postManagerMock.Object, commentFactoryMock.Object,
                commentManagerMock.Object, tagServiceMock.Object);

            var res = postService.GetPageCount(size);

            Assert.That(res, Is.EqualTo(expectedCount));
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        [TestCase(3, 3)]
        [TestCase(4, 4)]
        [TestCase(null, 2)]
        [TestCase(1, null)]
        public void GetPosts_All(int? pageIndex, int? pageSize)
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

            var commentFactoryMock = new Mock<ICommentFactory>();

            var commentManagerMock = new Mock<ICommentManager>();

            var postFactoryMock = new Mock<IPostFactory>();
            postFactoryMock.Setup(x => x.CreateModel(It.IsAny<Post>()))
                .Returns(new PostModel());

            var postManagerMock = new Mock<IPostManager>();
            postManagerMock.Setup(x => x.GetPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<Post, PostModel>>()))
                .Returns((int index, int size, Func<Post,PostModel> selector) 
                    => posts.Skip(index * size).Take(size).Select(selector));
    
            var tagServiceMock = new Mock<ITagService>();

            var postService = new PostService(postFactoryMock.Object, postManagerMock.Object, commentFactoryMock.Object,
                commentManagerMock.Object, tagServiceMock.Object);

            var request = new PagedRequest { PageIndex = pageIndex, PageSize = pageSize, SortingType = null };

            var res = postService.GetPosts(request);

            var expectedResult = posts.Skip((pageIndex ?? 0) * (pageSize ?? 15)).Take(pageSize ?? 15);

            Assert.That(res.Count(), Is.EqualTo(expectedResult.Count()));
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        [TestCase(3, 3)]
        [TestCase(4, 4)]
        [TestCase(null, 2)]
        [TestCase(1, null)]
        public void GetPosts_New(int? pageIndex, int? pageSize)
        {
            var posts = new List<Post>
            {
                new Post { Id = 1, Comment = new Comment { Id = 1, Created = DateTime.Now.AddDays(-1) } },
                new Post { Id = 2, Comment = new Comment { Id = 2, Created = DateTime.Now.AddDays(-2) } },
                new Post { Id = 3, Comment = new Comment { Id = 3, Created = DateTime.Now.AddDays(-4) } },
                new Post { Id = 4, Comment = new Comment { Id = 4, Created = DateTime.Now.AddDays(-3) } },
                new Post { Id = 5, Comment = new Comment { Id = 5, Created = DateTime.Now.AddDays(-1) } },
                new Post { Id = 6, Comment = new Comment { Id = 6, Created = DateTime.Now.AddDays(-2) } },
                new Post { Id = 7, Comment = new Comment { Id = 7, Created = DateTime.Now.AddDays(-1) } },
                new Post { Id = 8, Comment = new Comment { Id = 8, Created = DateTime.Now.AddDays(1) } },
                new Post { Id = 9, Comment = new Comment { Id = 9, Created = DateTime.Now.AddDays(-1) } },
            };

            var commentFactoryMock = new Mock<ICommentFactory>();

            var commentManagerMock = new Mock<ICommentManager>();

            var postFactoryMock = new Mock<IPostFactory>();
            postFactoryMock.Setup(x => x.CreateModel(It.IsAny<Post>()))
                .Returns((Post post) => new PostModel { Content = new CommentModel { Id = post.Comment.Id, Created = post.Comment.Created.ToString() } });

            var postManagerMock = new Mock<IPostManager>();
            postManagerMock.Setup(x => x.GetNewPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<Post, PostModel>>()))
                .Returns((int index, int size, Func<Post, PostModel> selector)
                    => posts.OrderBy(x => x.Comment.Created).Skip(index * size).Take(size).Select(selector));

            var tagServiceMock = new Mock<ITagService>();

            var postService = new PostService(postFactoryMock.Object, postManagerMock.Object, commentFactoryMock.Object,
                commentManagerMock.Object, tagServiceMock.Object);

            var request = new PagedRequest { PageIndex = pageIndex, PageSize = pageSize, SortingType = SortingType.New };

            var res = postService.GetPosts(request);

            var expectedResult = posts.OrderBy(x => x.Comment.Created)
                .Skip((pageIndex ?? 0) * (pageSize ?? 15))
                .Take(pageSize ?? 15);

            Assert.Multiple(() =>
            {
                Assert.That(res.Count(), Is.EqualTo(expectedResult.Count()));
                Assert.That(res.FirstOrDefault()?.Content.Created, Is.EqualTo(expectedResult.FirstOrDefault()?.Comment.Created.ToString()));
                Assert.That(res.FirstOrDefault()?.Content.Id, Is.EqualTo(expectedResult.FirstOrDefault()?.Comment?.Id));
            });
        }

        private List<CommentReaction> CreateReactions(int positive, int negative, int commentId)
        {
            var reaction = new List<CommentReaction>();
            for (int i = 0; i < positive; i++)
            {
                reaction.Add(new CommentReaction
                {
                    CommentId = commentId,
                    UserId = $"positive{i}",
                    Reaction = Reaction.Positive,
                });
            }
            for (int i = 0; i < negative; i++)
            {
                reaction.Add(new CommentReaction
                {
                    CommentId = commentId,
                    UserId = $"negative{i}",
                    Reaction = Reaction.Negative,
                });
            }

            return reaction;
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        [TestCase(3, 3)]
        [TestCase(4, 4)]
        [TestCase(null, 2)]
        [TestCase(1, null)]
        public void GetPosts_Top(int? pageIndex, int? pageSize)
        {
            var posts = new List<Post>
            {
                new Post { Id = 1, Comment = new Comment { Id = 1, Reactions = CreateReactions(3, 5, 1) } },
                new Post { Id = 2, Comment = new Comment { Id = 2, Reactions = CreateReactions(0, 10, 2) } },
                new Post { Id = 3, Comment = new Comment { Id = 3, Reactions = CreateReactions(9, 1, 3) } },
                new Post { Id = 4, Comment = new Comment { Id = 4, Reactions = CreateReactions(5, 0, 4) } },
                new Post { Id = 5, Comment = new Comment { Id = 5, Reactions = CreateReactions(8, 10, 5) } },
                new Post { Id = 6, Comment = new Comment { Id = 6, Reactions = CreateReactions(1, 0, 6) } },
                new Post { Id = 7, Comment = new Comment { Id = 7, Reactions = CreateReactions(2, 13, 7) } },
                new Post { Id = 8, Comment = new Comment { Id = 8, Reactions = CreateReactions(3, 6, 8) } },
                new Post { Id = 9, Comment = new Comment { Id = 9, Reactions = CreateReactions(5, 8, 9) } },
            };

            var commentFactoryMock = new Mock<ICommentFactory>();

            var commentManagerMock = new Mock<ICommentManager>();

            var postFactoryMock = new Mock<IPostFactory>();
            postFactoryMock.Setup(x => x.CreateModel(It.IsAny<Post>()))
                .Returns((Post post) => new PostModel { Content = new CommentModel { Id = post.Comment.Id } });

            var postManagerMock = new Mock<IPostManager>();
            postManagerMock.Setup(x => x.GetTopPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<Post, PostModel>>()))
                .Returns((int index, int size, Func<Post, PostModel> selector)
                    => posts.OrderBy(x => x.Comment.Reactions.SumReactions()).Skip(index * size).Take(size).Select(selector));

            var tagServiceMock = new Mock<ITagService>();

            var postService = new PostService(postFactoryMock.Object, postManagerMock.Object, commentFactoryMock.Object,
                commentManagerMock.Object, tagServiceMock.Object);

            var request = new PagedRequest { PageIndex = pageIndex, PageSize = pageSize, SortingType = SortingType.Top };

            var res = postService.GetPosts(request);

            var expectedResult = posts.OrderBy(x => x.Comment.Reactions.SumReactions())
                .Skip((pageIndex ?? 0) * (pageSize ?? 15))
                .Take(pageSize ?? 15);

            Assert.Multiple(() =>
            {
                Assert.That(res.Count(), Is.EqualTo(expectedResult.Count()));
                Assert.That(res.FirstOrDefault()?.Content.Id, Is.EqualTo(expectedResult.FirstOrDefault()?.Comment?.Id));
            });
        }

        [Test]
        public void Search_ByCreator()
        {
            var posts = new List<Post>
            {
                new Post { Id = 1, Comment = new Comment { Creator = new ApplicationUser { UserName = "name" } } },
                new Post { Id = 2, Comment = new Comment { Creator = new ApplicationUser { UserName = "name name" } } },
                new Post { Id = 3, Comment = new Comment { Creator = new ApplicationUser { UserName = "n" } } },
                new Post { Id = 4, Comment = new Comment { Creator = new ApplicationUser { UserName = "a" } } },
                new Post { Id = 5, Comment = new Comment { Creator = new ApplicationUser { UserName = "m" } } },
                new Post { Id = 6, Comment = new Comment { Creator = new ApplicationUser { UserName = "e" } } },
                new Post { Id = 7, Comment = new Comment { Creator = new ApplicationUser { UserName = "na" } } },
                new Post { Id = 8, Comment = new Comment { Creator = new ApplicationUser { UserName = "me" } } },
                new Post { Id = 9, Comment = new Comment { Creator = new ApplicationUser { UserName = "names" } } },
            };

            var commentFactoryMock = new Mock<ICommentFactory>();

            var commentManagerMock = new Mock<ICommentManager>();

            var postFactoryMock = new Mock<IPostFactory>();
            postFactoryMock.Setup(x => x.CreateModel(It.IsAny<Post>()))
                .Returns((Post post) => new PostModel { Content = new CommentModel { CreatorName = post.Comment.Creator.UserName } });

            var postManagerMock = new Mock<IPostManager>();
            postManagerMock.Setup(x => x.SearchPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<Func<Post, bool>>>(), It.IsAny<Func<Post, PostModel>>()))
                .Returns((int index, int size, IEnumerable<Func<Post, bool>> conditions, Func<Post, PostModel> selector)
                    => posts.Where(x => conditions.All(y => y(x))).Select(selector));

            var tagServiceMock = new Mock<ITagService>();

            var postService = new PostService(postFactoryMock.Object, postManagerMock.Object, commentFactoryMock.Object,
                commentManagerMock.Object, tagServiceMock.Object);

            var request = new SearchPostRequest
            {
                Text = "name",
                SearchCreator = true,
                PageIndex = 0,
                PageSize = 10
            };

            var res = postService.Search(request);

            Assert.Multiple(() =>
            {
                Assert.That(res.Count(), Is.EqualTo(3));
                Assert.That(res.All(x => x.Content.CreatorName.Contains(request.Text)));
            });
        }

        [Test]
        public void Search_ByTags()
        {
            var posts = new List<Post>
            {
                new Post { Id = 1, Tags = new List<PostTag> { new PostTag { Tag = new Tag { Name = "tagname" } } } },
                new Post { Id = 2, Tags = new List<PostTag> { new PostTag { Tag = new Tag { Name = "tagnametag" } } } },
                new Post { Id = 3, Tags = new List<PostTag> { new PostTag { Tag = new Tag { Name = "t" } } } },
                new Post { Id = 4, Tags = new List<PostTag> { new PostTag { Tag = new Tag { Name = "a" } } } },
                new Post { Id = 5, Tags = new List<PostTag> { new PostTag { Tag = new Tag { Name = "g" } } } },
                new Post { Id = 6, Tags = new List<PostTag> { new PostTag { Tag = new Tag { Name = "n" } } } },
                new Post { Id = 7, Tags = new List<PostTag> { new PostTag { Tag = new Tag { Name = "a" } } } },
                new Post { Id = 8, Tags = new List<PostTag> { new PostTag { Tag = new Tag { Name = "m" } } } },
                new Post { Id = 9, Tags = new List<PostTag> { new PostTag { Tag = new Tag { Name = "e" } } } },
            };

            var commentFactoryMock = new Mock<ICommentFactory>();

            var commentManagerMock = new Mock<ICommentManager>();

            var postFactoryMock = new Mock<IPostFactory>();
            postFactoryMock.Setup(x => x.CreateModel(It.IsAny<Post>()))
                .Returns((Post post) => new PostModel { TagList = post.Tags.Select(x => x.Tag) });

            var postManagerMock = new Mock<IPostManager>();
            postManagerMock.Setup(x => x.SearchPosts(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<IEnumerable<Func<Post, bool>>>(), It.IsAny<Func<Post, PostModel>>()))
                .Returns((int index, int size, IEnumerable<Func<Post, bool>> conditions, Func<Post, PostModel> selector)
                    => posts.Where(x => conditions.All(y => y(x))).Select(selector));

            var tagServiceMock = new Mock<ITagService>();

            var postService = new PostService(postFactoryMock.Object, postManagerMock.Object, commentFactoryMock.Object,
                commentManagerMock.Object, tagServiceMock.Object);

            var request = new SearchPostRequest
            {
                Text = "tagname",
                SearchTag = true,
                PageIndex = 0,
                PageSize = 10
            };

            var res = postService.Search(request);

            Assert.Multiple(() =>
            {
                Assert.That(res.Count(), Is.EqualTo(2));
                Assert.That(res.All(x => x.TagList.All(x => x.Name.Contains(request.Text))));
            });
        }

        [Test]
        public async Task RemovePostById()
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

            var commentFactoryMock = new Mock<ICommentFactory>();

            var commentManagerMock = new Mock<ICommentManager>();

            var postFactoryMock = new Mock<IPostFactory>();

            var postManagerMock = new Mock<IPostManager>();
            postManagerMock.Setup(x => x.RemovePostById(It.IsAny<int>()))
                .Callback((int id) => posts.Remove(posts.First(x => x.Id == id)))
                .ReturnsAsync(true);

            var tagServiceMock = new Mock<ITagService>();

            var postService = new PostService(postFactoryMock.Object, postManagerMock.Object, commentFactoryMock.Object,
                commentManagerMock.Object, tagServiceMock.Object);

            const int Id = 3;
            var res = await postService.RemovePostById(Id);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(!posts.Any(x => x.Id == Id));
            });
        }
    }
}
