using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopApi.Application.Tags;
using VikopApi.Application.Tags.Abtractions;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Services
{
    [TestFixture]
    public class TagServiceTests
    {
        [Test]
        public async Task CreatePost()
        {
            var tags = new List<Tag>();
            var postTags = new List<PostTag>();

            var random = new Random();
            var managerMock = new Mock<ITagManager>();
            managerMock.Setup(x => x.AddTags(It.IsAny<IEnumerable<string>>()))
                .Callback((IEnumerable<string> names) 
                    => tags.AddRange(names.Select(x => new Tag { Id = random.Next(1, 100), Name = x })))
                .ReturnsAsync(true);

            managerMock.Setup(x => x.GetTagsByNames(It.IsAny<IEnumerable<string>>()))
                .Returns((IEnumerable<string> names) => tags.Where(x => names.Contains(x.Name)));

            managerMock.Setup(x => x.AddTags(It.IsAny<IEnumerable<PostTag>>()))
                .Callback((IEnumerable<PostTag> tags) => postTags.AddRange(tags))
                .ReturnsAsync(true);

            var factoryMock = new Mock<ITagFactory>();
            factoryMock.Setup(x => x.CreatePost(It.IsAny<int>(), It.IsAny<IEnumerable<Tag>>()))
                .Returns((int id, IEnumerable<Tag> tags) => tags.Select(x => new PostTag { PostId = id, TagId = x.Id, Tag = x}));

            var service = new TagService(managerMock.Object, factoryMock.Object);

            var names = new List<string> { "name1", "name2", "name3", "name4" };
            const int postId = 1;

            var res = await service.CreatePost(names, postId);

            Assert.Multiple(() =>
            {
                Assert.That(tags.Count, Is.EqualTo(names.Count));
                Assert.That(postTags.Count, Is.EqualTo(names.Count));
                Assert.That(names.All(x => tags.Any(y => y.Name == x)));
                Assert.That(res, Is.EquivalentTo(tags));
            });
        }

        [Test]
        public async Task CreateFinding()
        {
            var tags = new List<Tag>();
            var findingTags = new List<FindingTag>();

            var random = new Random();
            var managerMock = new Mock<ITagManager>();
            managerMock.Setup(x => x.AddTags(It.IsAny<IEnumerable<string>>()))
                .Callback((IEnumerable<string> names)
                    => tags.AddRange(names.Select(x => new Tag { Id = random.Next(1, 100), Name = x })))
                .ReturnsAsync(true);

            managerMock.Setup(x => x.GetTagsByNames(It.IsAny<IEnumerable<string>>()))
                .Returns((IEnumerable<string> names) => tags.Where(x => names.Contains(x.Name)));

            managerMock.Setup(x => x.AddTags(It.IsAny<IEnumerable<FindingTag>>()))
                .Callback((IEnumerable<FindingTag> tags) => findingTags.AddRange(tags))
                .ReturnsAsync(true);

            var factoryMock = new Mock<ITagFactory>();
            factoryMock.Setup(x => x.CreateFinding(It.IsAny<int>(), It.IsAny<IEnumerable<Tag>>()))
                .Returns((int id, IEnumerable<Tag> tags) => tags.Select(x => new FindingTag { FindingId = id, TagId = x.Id, Tag = x }));

            var service = new TagService(managerMock.Object, factoryMock.Object);

            var names = new List<string> { "name1", "name2", "name3", "name4" };
            const int findingId = 1;

            var res = await service.CreateFinding(names, findingId);

            Assert.Multiple(() =>
            {
                Assert.That(tags.Count, Is.EqualTo(names.Count));
                Assert.That(findingTags.Count, Is.EqualTo(names.Count));
                Assert.That(names.All(x => tags.Any(y => y.Name == x)));
                Assert.That(res, Is.EquivalentTo(tags));
            });
        }
    }
}
