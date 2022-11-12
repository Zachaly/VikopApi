using NUnit.Framework.Interfaces;
using VikopApi.Database;
using VikopApi.Domain.Enums;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Managers
{
    [TestFixture]
    public class FindingManagerTests
    {
        [Test]
        public async Task AddFinding()
        {
            var finding = new Finding
            {
                Id = 1,
            };
            var dbContext = Extensions.GetAppDbContext();
            var manager = new FindingManager(dbContext);

            var res = await manager.AddFinding(finding);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(dbContext.Findings.Contains(finding));
                Assert.That(dbContext.Findings.Count(), Is.EqualTo(1));
            });
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        public void GetFindingById(int id)
        {
            var findings = new List<Finding>
            {
                new Finding { Id = 1 },
                new Finding { Id = 2 },
                new Finding { Id = 3 },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(findings);
            var manager = new FindingManager(dbContext);

            var res = manager.GetFindingById(id, x => x);

            Assert.That(res, Is.EqualTo(findings.FirstOrDefault(x => x.Id == id)));
        }

        [Test]
        [TestCase(0, 2)]
        [TestCase(1, 2)]
        [TestCase(2, 2)]
        [TestCase(1, 1)]
        [TestCase(2, 1)]
        [TestCase(3, 1)]
        [TestCase(4, 1)]
        public void GetAllFindings(int index, int size)
        {
            var findings = new List<Finding>
            {
                new Finding { Id = 1 },
                new Finding { Id = 2 },
                new Finding { Id = 3 },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(findings);
            var manager = new FindingManager(dbContext);

            var res = manager.GetAllFindings(index, size, x => x);

            Assert.That(res, Is.EquivalentTo(findings.Skip(index * size).Take(size)));
        }

        [Test]
        public void GetNewFindings()
        {
            var findings = new List<Finding>
            {
                new Finding { Id = 1, Created = new DateTime(2, 2, 2) },
                new Finding { Id = 2, Created = new DateTime(1, 1, 1) },
                new Finding { Id = 3, Created = new DateTime(3, 3, 3) },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(findings);
            var manager = new FindingManager(dbContext);

            var res = manager.GetNewFindings(0, 1, x => x);

            Assert.That(res.First(), Is.EqualTo(findings.OrderByDescending(x => x.Created).First()));
        }

        [Test]
        public void GetTopFindings_ReactionsCount()
        {
            var findings = new List<Finding>
            {
                new Finding 
                { 
                    Id = 1,
                    Created = DateTime.Now,
                    Reactions = new List<FindingReaction>
                    {
                        new FindingReaction { Reaction = Reaction.Negative, FindingId = 1, UserId = "id1" },
                        new FindingReaction { Reaction = Reaction.Negative, FindingId = 1, UserId = "id2" },
                        new FindingReaction { Reaction = Reaction.Negative, FindingId = 1, UserId = "id3" },
                    }
                },
                new Finding 
                { 
                    Id = 2,
                    Created = DateTime.Now,
                    Reactions = new List<FindingReaction>
                    {
                        new FindingReaction { Reaction = Reaction.Negative, FindingId = 2, UserId = "id1" },
                        new FindingReaction { Reaction = Reaction.Positive, FindingId = 2, UserId = "id2" },
                        new FindingReaction { Reaction = Reaction.Negative, FindingId = 2, UserId = "id3" },
                    }
                },
                new Finding 
                { 
                    Id = 3,
                    Created = DateTime.Now,
                    Reactions = new List<FindingReaction>
                    {
                        new FindingReaction { Reaction = Reaction.Positive, FindingId = 3, UserId = "id1" },
                        new FindingReaction { Reaction = Reaction.Positive, FindingId = 3, UserId = "id2" },
                        new FindingReaction { Reaction = Reaction.Positive, FindingId = 3, UserId = "id3" },
                    }
                },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(findings);
            var manager = new FindingManager(dbContext);

            var res = manager.GetTopFindings(0, 3, x => x);

            Assert.That(res, Is.EquivalentTo(findings.OrderByDescending(x => x.Reactions.Sum(y => (int)y.Reaction))));
        }

        [Test]
        public void GetTopFindings_CommentsCount()
        {
            var findings = new List<Finding>
            {
                new Finding
                {
                    Id = 1,
                    Created = DateTime.Now,
                    Comments = new List<FindingComment>
                    {
                        new FindingComment { CommentId = 1, FindingId = 1, Comment = new Comment{ Id = 1 } },
                        new FindingComment { CommentId = 2, FindingId = 1, Comment = new Comment{ Id = 2 } },
                        new FindingComment { CommentId = 3, FindingId = 1, Comment = new Comment{ Id = 3 } },
                    }
                },
                new Finding
                {
                    Id = 2,
                    Created = DateTime.Now,
                    Comments = new List<FindingComment>
                    {
                        new FindingComment { CommentId = 4, FindingId = 2, Comment = new Comment{ Id = 4 } },
                        new FindingComment { CommentId = 5, FindingId = 2, Comment = new Comment{ Id = 5 } },
                        new FindingComment { CommentId = 6, FindingId = 2, Comment = new Comment{ Id = 6 } },
                        new FindingComment { CommentId = 7, FindingId = 2, Comment = new Comment{ Id = 7 } },
                        new FindingComment { CommentId = 8, FindingId = 2, Comment = new Comment{ Id = 8 } },
                    }
                },
                new Finding
                {
                    Id = 3,
                    Created = DateTime.Now,
                    Comments = new List<FindingComment>
                    {
                        new FindingComment { CommentId = 9, FindingId = 3, Comment = new Comment{ Id = 9 } },
                        new FindingComment { CommentId = 10, FindingId = 3, Comment = new Comment{ Id = 10 } },
                        new FindingComment { CommentId = 11, FindingId = 3, Comment = new Comment{ Id = 11 } },
                        new FindingComment { CommentId = 12, FindingId = 3, Comment = new Comment{ Id = 12 } },
                    }
                },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(findings);
            var manager = new FindingManager(dbContext);

            var res = manager.GetTopFindings(0, 3, x => x);

            Assert.That(res, Is.EquivalentTo(findings.OrderBy(x => x.Comments.Count())));
        }

        [Test]
        public void GetTopFindings_DateCounts()
        {
            var findings = new List<Finding>
            {
                new Finding { Id = 1, Created = DateTime.Now.AddDays(-1) },
                new Finding { Id = 2, Created = DateTime.Now },
                new Finding { Id = 3, Created = DateTime.Now.AddMonths(-1) },
            };
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(findings);
            var manager = new FindingManager(dbContext);

            var res = manager.GetTopFindings(0, 3, x => x);

            Assert.That(res, Is.EquivalentTo(findings.OrderByDescending(x => x.Created)));
        }

        [Test]
        [TestCase(2, 5)]
        [TestCase(3, 3)]
        [TestCase(1, 9)]
        [TestCase(8, 2)]
        [TestCase(5, 2)]
        public void GetPageCount(int pageSize, int expectedCount)
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
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(findings);
            var manager = new FindingManager(dbContext);

            var res = manager.GetPageCount(pageSize);

            Assert.That(res, Is.EqualTo(expectedCount));
        }

        [Test]
        public void SearchFindings()
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
            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(findings);
            var manager = new FindingManager(dbContext);
            var conditions = new List<Func<Finding, bool>>();

            conditions.Add(finding => finding.Id == 2);

            var res = manager.SearchFindings(0, 10, conditions, x => x);

            Assert.That(res, Is.EquivalentTo(findings.Where(x => x.Id == 2)));
        }

        [Test]
        public async Task RemoveFindingById()
        {
            var findings = new List<Finding>
            {
                new Finding { Id = 1 },
                new Finding { Id = 2 },
                new Finding { Id = 3 },
                new Finding { Id = 4 },
            };

            var comments = new List<FindingComment>
            {
                new FindingComment { CommentId = 1, FindingId = 2 },
                new FindingComment { CommentId = 2, FindingId = 2 },
                new FindingComment { CommentId = 3, FindingId = 2 }
            };

            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(findings);
            dbContext.AddContent(comments);

            var manager = new FindingManager(dbContext);

            const int Id = 2;
            var res = await manager.RemoveFindingById(Id);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(!dbContext.Findings.Any(x => x.Id == Id));
                Assert.That(!dbContext.FindingComments.Any(x => x.FindingId == Id));
            });
        }
    }
}
