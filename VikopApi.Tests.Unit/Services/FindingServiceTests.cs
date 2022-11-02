using Moq;
using VikopApi.Application;
using VikopApi.Application.Findings;
using VikopApi.Application.Findings.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Tags.Abtractions;
using VikopApi.Domain.Enums;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Services
{
    [TestFixture]
    public class FindingServiceTests
    {
        [Test]
        public async Task AddFinding()
        {
            var random = new Random();
            var findings = new List<Finding>();
            var findingTags = new List<FindingTag>();
            var tags = new List<Tag>();

            var findingManagerMock = new Mock<IFindingManager>();
            findingManagerMock.Setup(x => x.AddFinding(It.IsAny<Finding>()))
                .Callback((Finding finding) =>
                {
                    finding.Id = random.Next(1, 100);
                    findings.Add(finding);
                }).ReturnsAsync(true);

            var tagServiceMock = new Mock<ITagService>();
            tagServiceMock.Setup(x => x.CreateFinding(It.IsAny<IEnumerable<string>>(), It.IsAny<int>()))
                .Callback((IEnumerable<string> names, int id) =>
                {
                    foreach (var name in names)
                    {
                        var tag = new Tag
                        {
                            Id = random.Next(1, 100),
                            Name = name
                        };
                        tags.Add(tag);
                        findingTags.Add(new FindingTag { FindingId = id, Tag = tag, TagId = tag.Id });
                    }
                });

            var factoryMock = new Mock<IFindingFactory>();
            factoryMock.Setup(x => x.Create(It.IsAny<AddFindingRequest>()))
                .Returns((AddFindingRequest request) => new Finding 
                { 
                    CreatorId = request.CreatorId,
                    Description = request.Description,
                    Title = request.Title,
                    Link = request.Link,
                    Picture = request.Picture,
                });

            var service = new FindingService(factoryMock.Object, findingManagerMock.Object, tagServiceMock.Object);
            var request = new AddFindingRequest
            {
                CreatorId = "id",
                Description = "desc",
                Link = "link",
                Picture = "pic",
                TagList = new List<string> { "xd", "lol", "bruh" },
                Title = "title",
            };

            var res = await service.AddFinding(request);

            Assert.Multiple(() =>
            {
                Assert.That(findings.Count, Is.EqualTo(1));
                Assert.That(findings.Any(x => x.Id == res));
                Assert.That(request.TagList.All(x => tags.Any(y => y.Name == x)));
                Assert.That(request.TagList.Count(), Is.EqualTo(findingTags.Where(x => x.FindingId == res).Count()));
            });
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        [TestCase(3, 3)]
        [TestCase(4, 4)]
        [TestCase(null, 2)]
        [TestCase(1, null)]
        public void GetFindings_All(int? pageIndex, int? pageSize)
        {
            var findings = new List<Finding>
            {
                new Finding { Id = 1 },
                new Finding { Id = 2 },
                new Finding { Id = 3 },
                new Finding { Id = 4 },
                new Finding { Id = 5 },
                new Finding { Id = 6 },
                new Finding { Id = 7 },
                new Finding { Id = 8 },
                new Finding { Id = 9 },
            };

            var managerMock = new Mock<IFindingManager>();
            managerMock.Setup(x => x.GetAllFindings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<Finding, FindingListItemModel>>()))
                .Returns((int index, int size, Func<Finding, FindingListItemModel> selector)
                    => findings.Skip(index * size).Take(size).Select(selector));

            var factoryMock = new Mock<IFindingFactory>();
            factoryMock.Setup(x => x.CreateListItem(It.IsAny<Finding>()))
                .Returns((Finding finding) => new FindingListItemModel { Id = finding.Id });

            var tagServiceMock = new Mock<ITagService>();

            var service = new FindingService(factoryMock.Object, managerMock.Object, tagServiceMock.Object);

            var res = service.GetFindings(null, pageIndex, pageSize);

            var expectedResult = findings.Skip((pageSize ?? 10) * (pageIndex ?? 0))
                .Take(pageSize ?? 10)
                .Select(x => new FindingListItemModel { Id = x.Id });

            Assert.That(res.Count(), Is.EqualTo(expectedResult.Count()));
        }

        [Test]
        [TestCase(1, 2)]
        [TestCase(2, 1)]
        [TestCase(3, 3)]
        [TestCase(4, 4)]
        [TestCase(null, 2)]
        [TestCase(1, null)]
        public void GetFindings_New(int? pageIndex, int? pageSize)
        {
            var findings = new List<Finding>
            {
                new Finding { Id = 1, Created = DateTime.Now.AddDays(-1) },
                new Finding { Id = 2, Created = DateTime.Now.AddDays(-10) },
                new Finding { Id = 3, Created = DateTime.Now.AddDays(-3) },
                new Finding { Id = 4, Created = DateTime.Now.AddDays(-5) },
                new Finding { Id = 5, Created = DateTime.Now.AddDays(-2) },
                new Finding { Id = 6, Created = DateTime.Now.AddDays(-3) },
                new Finding { Id = 7, Created = DateTime.Now.AddDays(1) },
                new Finding { Id = 8, Created = DateTime.Now.AddDays(9) },
                new Finding { Id = 9, Created = DateTime.Now.AddDays(-20) },
            };

            var managerMock = new Mock<IFindingManager>();
            managerMock.Setup(x => x.GetNewFindings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<Finding, FindingListItemModel>>()))
                .Returns((int index, int size, Func<Finding, FindingListItemModel> selector)
                    => findings.OrderByDescending(x => x.Created).Skip(index * size).Take(size).Select(selector));

            var factoryMock = new Mock<IFindingFactory>();
            factoryMock.Setup(x => x.CreateListItem(It.IsAny<Finding>()))
                .Returns((Finding finding) => new FindingListItemModel { Id = finding.Id });

            var tagServiceMock = new Mock<ITagService>();

            var service = new FindingService(factoryMock.Object, managerMock.Object, tagServiceMock.Object);

            var res = service.GetFindings(SortingType.New, pageIndex, pageSize);

            var expectedResult = findings.OrderByDescending(x => x.Created)
                .Skip((pageSize ?? 10) * (pageIndex ?? 0))
                .Take(pageSize ?? 10)
                .Select(x => new FindingListItemModel { Id = x.Id });

            Assert.Multiple(() =>
            {
                Assert.That(res.Count(), Is.EqualTo(expectedResult.Count()));
                Assert.That(res.All(x => expectedResult.Any(y => y.Id == x.Id)));
                Assert.That(res.FirstOrDefault()?.Id, Is.EqualTo(expectedResult.FirstOrDefault()?.Id));
            });
        }

        private List<FindingReaction> CreateReactions(int positive, int negative, int findingId)
        {
            var reaction = new List<FindingReaction>();
            for(int i = 0; i < positive; i++)
            {
                reaction.Add(new FindingReaction
                {
                    FindingId = findingId,
                    UserId = $"positive{i}",
                    Reaction = Reaction.Positive,
                });
            }
            for (int i = 0; i < negative; i++)
            {
                reaction.Add(new FindingReaction
                {
                    FindingId = findingId,
                    UserId = $"newgative{i}",
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
        public void GetFindings_Top(int? pageIndex, int? pageSize)
        {
            var findings = new List<Finding>
            {
                new Finding { Id = 1, Reactions = CreateReactions(1, 2, 1) },
                new Finding { Id = 2, Reactions = CreateReactions(10, 2, 2) },
                new Finding { Id = 3, Reactions = CreateReactions(0, 2, 3) },
                new Finding { Id = 4, Reactions = CreateReactions(15, 0, 4) },
                new Finding { Id = 5, Reactions = CreateReactions(0, 20, 5) },
                new Finding { Id = 6, Reactions = CreateReactions(0, 3, 6) },
                new Finding { Id = 7, Reactions = CreateReactions(5, 1, 7) },
                new Finding { Id = 8, Reactions = CreateReactions(1, 0, 8) },
                new Finding { Id = 9, Reactions = CreateReactions(21, 20, 9) },
            };

            var managerMock = new Mock<IFindingManager>();
            managerMock.Setup(x => x.GetTopFindings(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<Finding, FindingListItemModel>>()))
                .Returns((int index, int size, Func<Finding, FindingListItemModel> selector)
                    => findings.OrderByDescending(x => x.Reactions.SumReactions()).Skip(index * size).Take(size).Select(selector));

            var factoryMock = new Mock<IFindingFactory>();
            factoryMock.Setup(x => x.CreateListItem(It.IsAny<Finding>()))
                .Returns((Finding finding) => new FindingListItemModel { Id = finding.Id });

            var tagServiceMock = new Mock<ITagService>();

            var service = new FindingService(factoryMock.Object, managerMock.Object, tagServiceMock.Object);

            var res = service.GetFindings(SortingType.Top, pageIndex, pageSize);

            var expectedResult = findings.OrderByDescending(x => x.Reactions.SumReactions())
                .Skip((pageSize ?? 10) * (pageIndex ?? 0))
                .Take(pageSize ?? 10)
                .Select(x => new FindingListItemModel { Id = x.Id });

            Assert.Multiple(() =>
            {
                Assert.That(res.Count(), Is.EqualTo(expectedResult.Count()));
                Assert.That(res.All(x => expectedResult.Any(y => y.Id == x.Id)));
                Assert.That(res.FirstOrDefault()?.Id, Is.EqualTo(expectedResult.FirstOrDefault()?.Id));
            });
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void GetFindingById(int findingId)
        {
            var findings = new List<Finding>
            {
                new Finding { Id = 1 },
                new Finding { Id = 2 },
                new Finding { Id = 3 }
            };

            var managerMock = new Mock<IFindingManager>();
            managerMock.Setup(x => x.GetFindingById(It.IsAny<int>(), It.IsAny<Func<Finding, FindingModel>>()))
                .Returns((int id, Func<Finding, FindingModel> selector) 
                    => findings.Where(x => x.Id == id).Select(selector).FirstOrDefault());

            var factoryMock = new Mock<IFindingFactory>();
            factoryMock.Setup(x => x.CreateModel(It.IsAny<Finding>()))
                .Returns((Finding finding) => new FindingModel { Finding = new FindingListItemModel { Id = finding.Id } });

            var tagServiceMock = new Mock<ITagService>();

            var service = new FindingService(factoryMock.Object, managerMock.Object, tagServiceMock.Object);

            var res = service.GetFindingById(findingId);

            Assert.That(res.Finding.Id, Is.EqualTo(findingId));
        }

        [Test]
        [TestCase(2, 5)]
        [TestCase(3, 3)]
        [TestCase(1, 9)]
        [TestCase(8, 2)]
        [TestCase(5, 2)]
        public void GetPageCount(int size, int expectedCount)
        {
            var findings = new List<Finding>
            {
                new Finding { Id = 1 },
                new Finding { Id = 2 },
                new Finding { Id = 3 },
                new Finding { Id = 4 },
                new Finding { Id = 5 },
                new Finding { Id = 6 },
                new Finding { Id = 7 },
                new Finding { Id = 8 },
                new Finding { Id = 9 },
            };

            var managerMock = new Mock<IFindingManager>();
            managerMock.Setup(x => x.GetPageCount(It.IsAny<int>()))
                .Returns((int x) => (int)Math.Ceiling(findings.Count / (decimal)x));

            var factoryMock = new Mock<IFindingFactory>();

            var tagServiceMock = new Mock<ITagService>();

            var service = new FindingService(factoryMock.Object, managerMock.Object, tagServiceMock.Object);

            var result = service.GetPageCount(size);

            Assert.That(result, Is.EqualTo(expectedCount));
        }
    }
}
