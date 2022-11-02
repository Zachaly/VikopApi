using VikopApi.Application.Tags;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.FactoryTests
{
    [TestFixture]
    public class TagFactoryTests
    {
        [Test]
        public void Create()
        {
            var factory = new TagFactory();
            var tags = new List<string> { "tag1", "tag2", "tag3" };

            var result = factory.Create(tags);

            Assert.That(result.Select(tag => tag.Name), Is.EquivalentTo(tags));
        }

        [Test]
        public void CreateFinding()
        {
            var factory = new TagFactory();
            var tags = new List<Tag>
            {
                new Tag
                {
                    Name = "tag1",
                    Id = 1,
                },
                new Tag
                {
                    Name = "tag2",
                    Id = 2,
                }
            };
            const int FindingId = 3;

            var findingTags = factory.CreateFinding(FindingId, tags);

            Assert.Multiple(() =>
            {
                Assert.That(findingTags.All(tag => tag.FindingId == FindingId));
                Assert.That(findingTags.Select(tag => tag.TagId), Is.EquivalentTo(tags.Select(tag => tag.Id)));
            });
        }

        [Test]
        public void CreateComment()
        {
            var factory = new TagFactory();
            var tags = new List<Tag>
            {
                new Tag
                {
                    Name = "tag1",
                    Id = 1,
                },
                new Tag
                {
                    Name = "tag2",
                    Id = 2,
                }
            };
            const int PostId = 3;

            var postTags = factory.CreatePost(PostId, tags);

            Assert.Multiple(() =>
            {
                Assert.That(postTags.All(tag => tag.PostId == PostId));
                Assert.That(postTags.Select(tag => tag.TagId), Is.EquivalentTo(tags.Select(tag => tag.Id)));
            });
        }
    }
}
