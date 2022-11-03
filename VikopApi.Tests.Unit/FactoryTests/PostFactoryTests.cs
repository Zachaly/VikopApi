using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using VikopApi.Application;
using VikopApi.Application.Comments;
using VikopApi.Application.Posts;
using VikopApi.Domain.Enums;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.FactoryTests
{
    [TestFixture]
    public class PostFactoryTests
    {
        [Test]
        public void Create_ByComment()
        {
            var commentFactory = new CommentFactory();
            var factory = new PostFactory(commentFactory);
            var comment = new Comment
            {
                Id = 1,
            };

            var post = factory.Create(comment);

            Assert.That(post.CommentId, Is.EqualTo(comment.Id));
        }

        [Test]
        public void CreateModel_ByPost()
        {
            var commentFactory = new CommentFactory();
            var factory = new PostFactory(commentFactory);
            var post = new Post
            {
                Comment = new Comment
                {
                    Id = 1,
                    Content = "content",
                    Created = new DateTime(1, 1, 1),
                    Creator = new ApplicationUser { Id = "xd", UserName = "name", Rank = Rank.Green },
                    CreatorId = "xd",
                    Picture = "pic",
                    Reactions = new List<CommentReaction>()
                },
                Tags = new List<PostTag> { new PostTag { Tag = new Tag { Id = 1, Name = "tag" } } }
            };

            var model = factory.CreateModel(post);

            Assert.Multiple(() =>
            {
                Assert.That(model.Content.HasPicture, Is.True);
                Assert.That(model.Content.Id, Is.EqualTo(post.Comment.Id));
                Assert.That(model.Content.CreatorRank, Is.EqualTo((int)post.Comment.Creator.Rank));
                Assert.That(model.Content.CreatorId, Is.EqualTo(post.Comment.CreatorId));
                Assert.That(model.Content.Content, Is.EqualTo(post.Comment.Content));
                Assert.That(model.Content.Created, Is.EqualTo(post.Comment.Created.GetTime()));
                Assert.That(model.Content.CreatorName, Is.EqualTo(post.Comment.Creator.UserName));
                Assert.That(model.Content.Reactions, Is.EqualTo(post.Comment.Reactions.SumReactions()));
                Assert.That(model.TagList, Is.EquivalentTo(post.Tags.Select(tag => tag.Tag)));
            });
        }

        [Test]
        public void CreateModel_CommentAndTags()
        {
            var commentFactory = new CommentFactory();
            var factory = new PostFactory(commentFactory);
            var comment = new Comment
            {
                Id = 1,
                Content = "content",
                Created = new DateTime(1, 1, 1),
                Creator = new ApplicationUser { Id = "xd", UserName = "name", Rank = Rank.Green },
                CreatorId = "xd",
                Picture = "pic",
                Reactions = new List<CommentReaction>()
            };

            var tags = new List<Tag> { new Tag { Id = 1, Name = "tag" } };
            

            var model = factory.CreateModel(comment, tags);

            Assert.Multiple(() =>
            {
                Assert.That(model.Content.HasPicture, Is.True);
                Assert.That(model.Content.Id, Is.EqualTo(comment.Id));
                Assert.That(model.Content.CreatorRank, Is.EqualTo((int)comment.Creator.Rank));
                Assert.That(model.Content.CreatorId, Is.EqualTo(comment.CreatorId));
                Assert.That(model.Content.Content, Is.EqualTo(comment.Content));
                Assert.That(model.Content.Created, Is.EqualTo(comment.Created.GetTime()));
                Assert.That(model.Content.CreatorName, Is.EqualTo(comment.Creator.UserName));
                Assert.That(model.Content.Reactions, Is.EqualTo(comment.Reactions.SumReactions()));
                Assert.That(model.TagList, Is.EquivalentTo(tags));
            });
        }
    }
}
