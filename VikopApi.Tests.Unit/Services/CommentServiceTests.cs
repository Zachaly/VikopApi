using Moq;
using VikopApi.Application.Comments;
using VikopApi.Application.Comments.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Services
{
    [TestFixture]
    public class CommentServiceTests
    {
        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task AddFindingComment(int findingId)
        {
            var random = new Random();
            var comments = new List<Comment>();
            var findingComments = new List<FindingComment>();
            var findings = new List<Finding>
            {
                new Finding { Id = 1 },
                new Finding { Id = 2 },
                new Finding { Id = 3 }
            };
            var managerMock = new Mock<ICommentManager>();
            managerMock.Setup(x => x.AddFindingComment(It.IsAny<int>(), It.IsAny<int>()))
                .Callback((int commentId, int findingId) => 
                {
                    findingComments.Add(new FindingComment { CommentId = commentId, FindingId = findingId });
                }).ReturnsAsync(true);

            managerMock.Setup(x => x.AddComment(It.IsAny<Comment>()))
                .Callback((Comment comment) => 
                { 
                    comment.Id = random.Next(1, 100);
                    comments.Add(comment); 
                }).ReturnsAsync(true);

            managerMock.Setup(x => x.GetCommentById(It.IsAny<int>(), It.IsAny<Func<Comment, CommentModel>>()))
                .Returns((int id, Func<Comment, CommentModel> selector) => comments.Select(selector).FirstOrDefault(x => x.Id == id));

            var factoryMock = new Mock<ICommentFactory>();
            factoryMock.Setup(x => x.Create(It.IsAny<AddCommentRequest>()))
                .Returns((AddCommentRequest request) => new Comment { CreatorId = request.CreatorId, Content = request.Content });

            factoryMock.Setup(x => x.CreateModel(It.IsAny<Comment>()))
                .Returns((Comment comment) => new CommentModel { Id = comment.Id });

            var service = new CommentService(managerMock.Object, factoryMock.Object);
            var request = new AddFindingCommentRequest
            {
                Content = "content",
                CreatorId = "id1",
                FindingId = findingId,
            };

            var res = await service.AddFindingComment(request);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.Not.Null);
                Assert.That(comments.Any(x => x.Id == res.Id && x.Content == request.Content && x.CreatorId == request.CreatorId));
                Assert.That(findingComments.Any(x => x.CommentId == res.Id));
                Assert.That(findingComments.Count, Is.EqualTo(1));
                Assert.That(comments.Count, Is.EqualTo(1));
            });
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task AddSubcomment(int commentId)
        {
            var random = new Random();
            var comments = new List<Comment>
            {
                new Comment { Id = 1 },
                new Comment { Id = 2 },
                new Comment { Id = 3 }
            };
            var subcomments = new List<SubComment>();
            var managerMock = new Mock<ICommentManager>();

            managerMock.Setup(x => x.AddComment(It.IsAny<Comment>()))
                .Callback((Comment comment) =>
                {
                    comment.Id = random.Next(1, 100);
                    comments.Add(comment);
                }).ReturnsAsync(true);

            managerMock.Setup(x => x.GetCommentById(It.IsAny<int>(), It.IsAny<Func<Comment, CommentModel>>()))
                .Returns((int id, Func<Comment, CommentModel> selector) => comments.Select(selector).FirstOrDefault(x => x.Id == id));

            managerMock.Setup(x => x.AddSubComment(It.IsAny<SubComment>()))
                .Callback((SubComment comment) => subcomments.Add(comment))
                .ReturnsAsync(true);

            var factoryMock = new Mock<ICommentFactory>();
            factoryMock.Setup(x => x.Create(It.IsAny<AddCommentRequest>()))
                .Returns((AddCommentRequest request) => new Comment { CreatorId = request.CreatorId, Content = request.Content });

            factoryMock.Setup(x => x.CreateSubComment(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int subcommentId, int commentId) => new SubComment { CommentId = subcommentId, MainCommentId = commentId });
                
            factoryMock.Setup(x => x.CreateModel(It.IsAny<Comment>()))
                .Returns((Comment comment) => new CommentModel { Id = comment.Id });

            var service = new CommentService(managerMock.Object, factoryMock.Object);
            var request = new AddSubcommentRequest
            {
                MainCommentId = commentId,
                Content = "content",
                CreatorId = "id1",
                Picture = "pic"
            };

            var res = await service.AddSubcomment(request);

            Assert.Multiple(() =>
            {
                Assert.That(subcomments.Any(x => x.CommentId == res.Id && x.MainCommentId == request.MainCommentId));
                Assert.That(comments.Any(x => x.Id == res.Id && x.Content == request.Content && x.CreatorId == request.CreatorId));
                Assert.That(comments.Count, Is.EqualTo(4));
                Assert.That(subcomments.Count, Is.EqualTo(1));
            });
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetCommentById(int id)
        {
            var comments = new List<Comment>
            {
                new Comment { Id = 1, Content = "content1" },
                new Comment { Id = 2, Content = "content2" },
                new Comment { Id = 3, Content = "content3" }
            };

            var managerMock = new Mock<ICommentManager>();
            managerMock.Setup(x => x.GetCommentById(It.IsAny<int>(), It.IsAny<Func<Comment, CommentModel>>()))
                .Returns((int id, Func<Comment, CommentModel> selector) => comments.Select(selector).FirstOrDefault(x => x.Id == id));

            var factoryMock = new Mock<ICommentFactory>();
            factoryMock.Setup(x => x.CreateModel(It.IsAny<Comment>()))
                .Returns((Comment comment) => new CommentModel { Id = comment.Id, Content = comment.Content});

            var service = new CommentService(managerMock.Object, factoryMock.Object);

            var res = service.GetCommentById(id);

            Assert.Multiple(() =>
            {
                Assert.That(res.Id, Is.EqualTo(id));
                Assert.That(res.Content, Is.EqualTo(comments.FirstOrDefault(x => x.Id == id).Content));
            });
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetSubComments(int id)
        {
            var comments = new List<Comment>
            {
                new Comment 
                { 
                    Id = 1,
                    Content = "content1",
                    SubComments = new List<SubComment>
                    {
                        new SubComment
                        {
                            Comment = new Comment
                            {
                                Id = 10,
                                Content = "content10"
                            },
                            CommentId = 10,
                            MainCommentId = 1
                        },
                        new SubComment
                        {
                            Comment = new Comment
                            {
                                Id = 11,
                                Content = "content11"
                            },
                            CommentId = 11,
                            MainCommentId = 1
                        },
                    }
                },
                new Comment 
                { 
                    Id = 2, 
                    Content = "content2",
                    SubComments = new List<SubComment>
                    {
                        new SubComment
                        {
                            Comment = new Comment
                            {
                                Id = 12,
                                Content = "content12"
                            },
                            CommentId = 12,
                            MainCommentId = 2
                        },
                        new SubComment
                        {
                            Comment = new Comment
                            {
                                Id = 13,
                                Content = "content13"
                            },
                            CommentId = 13,
                            MainCommentId = 2
                        },
                        new SubComment
                        {
                            Comment = new Comment
                            {
                                Id = 14,
                                Content = "content14"
                            },
                            CommentId = 14,
                            MainCommentId = 2
                        },
                    }
                },
                new Comment 
                {
                    Id = 3,
                    Content = "content3",
                    SubComments = new List<SubComment>
                    {
                        new SubComment
                        {
                            Comment = new Comment
                            {
                                Id = 15,
                                Content = "content15"
                            },
                            CommentId = 15,
                            MainCommentId = 3
                        },
                    }
                }
            };

            var managerMock = new Mock<ICommentManager>();

            managerMock.Setup(x => x.GetSubComments(It.IsAny<int>(), It.IsAny<Func<SubComment, CommentModel>>()))
                .Returns((int id, Func<SubComment, CommentModel> selector) 
                    => comments.FirstOrDefault(x => x.Id == id).SubComments.Select(selector));

            var factoryMock = new Mock<ICommentFactory>();
            factoryMock.Setup(x => x.CreateModel(It.IsAny<Comment>()))
                .Returns((Comment comment) => new CommentModel { Id = comment.Id, Content = comment.Content });

            var service = new CommentService(managerMock.Object, factoryMock.Object);

            var res = service.GetSubcomments(id);

            Assert.Multiple(() =>
            {
                Assert.That(res.Count(), Is.EqualTo(comments.FirstOrDefault(x => x.Id == id).SubComments.Count));
                Assert.That(res.All(x => comments.FirstOrDefault(y => y.Id == id).SubComments.Any(y => y.CommentId == x.Id)));
            });
        }
    }
}
