using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopApi.Database;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Managers
{
    [TestFixture]
    public class TagManagerTests
    {
        [Test]
        public async Task AddTags_Finding()
        {
            var dbContext = Extensions.GetAppDbContext();
            var manager = new TagManager(dbContext);
            var tags = new List<FindingTag>
            {
                new FindingTag { FindingId = 1, TagId = 1 },
                new FindingTag { FindingId = 2, TagId = 2 },
                new FindingTag { FindingId = 3, TagId = 3 },
            };

            var res = await manager.AddTags(tags);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(tags.All(tag => dbContext.FindingTags.Contains(tag)));
            });
        }

        [Test]
        public async Task AddTags_Post()
        {
            var dbContext = Extensions.GetAppDbContext();
            var manager = new TagManager(dbContext);
            var tags = new List<PostTag>
            {
                new PostTag { PostId = 1, TagId = 1 },
                new PostTag { PostId = 2, TagId = 2 },
                new PostTag { PostId = 3, TagId = 3 },
            };

            var res = await manager.AddTags(tags);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(tags.All(tag => dbContext.PostTags.Contains(tag)));
            });
        }

        [Test]
        public async Task AddTags_Names_TagsDoNotExist()
        {
            var names = new List<string> { "tag1", "tag2", "tag3", "tag4" };
            var dbContext = Extensions.GetAppDbContext();
            var manager = new TagManager(dbContext);

            var res = await manager.AddTags(names);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(names.All(name => dbContext.Tags.Any(x => x.Name == name)));
                Assert.That(dbContext.Tags.Count(), Is.EqualTo(names.Count));
            });
        }

        [Test]
        public async Task AddTags_Names_SomeTagsExist()
        {
            var names = new List<string> { "tag1", "tag2", "tag3", "tag4" };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(new List<Tag> { new Tag { Id = 1, Name = names[0] }, new Tag { Id = 2, Name = names[1] } });
            var manager = new TagManager(dbContext);

            var res = await manager.AddTags(names);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(names.All(name => dbContext.Tags.Any(x => x.Name == name)));
                Assert.That(dbContext.Tags.Count(), Is.EqualTo(names.Count));
            });
        }

        [Test]
        public async Task AddTags_Names_AllTagsExist()
        {
            var names = new List<string> { "tag1", "tag2", "tag3", "tag4" };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(names.Select(x => new Tag { Name = x }).ToList());
            var manager = new TagManager(dbContext);

            var res = await manager.AddTags(names);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(names.All(name => dbContext.Tags.Any(x => x.Name == name)));
                Assert.That(dbContext.Tags.Count(), Is.EqualTo(names.Count));
            });
        }

        [Test]
        public void GetTagsByNames()
        {
            var names = new List<string> { "tag1", "tag2", "tag3", "tag4" };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(names.Select(x => new Tag { Name = x }).ToList());
            var manager = new TagManager(dbContext);

            var res = manager.GetTagsByNames(names);

            Assert.That(res.Select(tag => tag.Name), Is.EquivalentTo(names));
        }
    }
}
