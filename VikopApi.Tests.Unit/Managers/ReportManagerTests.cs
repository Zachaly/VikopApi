using VikopApi.Database;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Managers
{
    [TestFixture]
    public class ReportManagerTests
    {
        [Test]
        public void GetReports_Post()
        {
            var reports = new List<PostReport>
            {
                new PostReport { Id = 1, Created = DateTime.Now },
                new PostReport { Id = 2, Created = DateTime.Now.AddDays(1) },
                new PostReport { Id = 3, Created = DateTime.Now.AddDays(-1) },
                new PostReport { Id = 4, Created = DateTime.Now.AddMonths(-1) },
            };

            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(reports);

            var manager = new ReportManager(dbContext);

            var res = manager.GetReports(0, 2, (PostReport report) => report);

            Assert.Multiple(() =>
            {
                Assert.That(res.Count(), Is.EqualTo(2));
                Assert.That(res, Is.EquivalentTo(reports.OrderByDescending(x => x.Created).Take(2)));
            });
        }

        [Test]
        public void GetReports_Finding()
        {
            var reports = new List<FindingReport>
            {
                new FindingReport { Id = 1, Created = DateTime.Now },
                new FindingReport { Id = 2, Created = DateTime.Now.AddDays(1) },
                new FindingReport { Id = 3, Created = DateTime.Now.AddDays(-1) },
                new FindingReport { Id = 4, Created = DateTime.Now.AddMonths(-1) },
            };

            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(reports);

            var manager = new ReportManager(dbContext);

            var res = manager.GetReports(0, 2, (FindingReport report) => report);

            Assert.Multiple(() =>
            {
                Assert.That(res.Count(), Is.EqualTo(2));
                Assert.That(res, Is.EquivalentTo(reports.OrderByDescending(x => x.Created).Take(2)));
            });
        }

        [Test]
        public async Task AddReport_Post()
        {
            var dbContext = Extensions.GetAppDbContext();

            var manager = new ReportManager(dbContext);
            var report = new PostReport
            {
                Created = DateTime.Now,
                PostId = 1,
                ReportingUserId = "id",
                Reason = "reason"
            };

            var res = await manager.AddReport(report);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(dbContext.PostReports.Contains(report));
            });
        }

        [Test]
        public async Task AddReport_Finding()
        {
            var dbContext = Extensions.GetAppDbContext();

            var manager = new ReportManager(dbContext);
            var report = new FindingReport
            {
                Created = DateTime.Now,
                FindingId = 1,
                ReportingUserId = "id",
                Reason = "reason"
            };

            var res = await manager.AddReport(report);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(dbContext.FindingReports.Contains(report));
            });
        }

        [Test]
        public async Task RemoveReport_Post()
        {
            var reports = new List<PostReport>
            {
                new PostReport { Id = 1, PostId = 1 },
                new PostReport { Id = 2, PostId = 2 },
                new PostReport { Id = 3, PostId = 2 },
                new PostReport { Id = 4, PostId = 3 },
            };

            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(reports);

            var manager = new ReportManager(dbContext);
            const int PostId = 2;
            var res = await manager.RemovePostReport(PostId);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(!dbContext.PostReports.Any(x => x.PostId == PostId));
            });
        }

        [Test]
        public async Task RemoveReport_Finding()
        {
            var reports = new List<FindingReport>
            {
                new FindingReport { Id = 1, FindingId = 1 },
                new FindingReport { Id = 2, FindingId = 2 },
                new FindingReport { Id = 3, FindingId = 2 },
                new FindingReport { Id = 4, FindingId = 3 },
            };

            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(reports);

            var manager = new ReportManager(dbContext);
            const int FindingId = 2;
            var res = await manager.RemoveFindingReport(FindingId);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(!dbContext.FindingReports.Any(x => x.FindingId == FindingId));
            });
        }

        [Test]
        public void GetReportById_Post()
        {
            var reports = new List<PostReport>
            {
                new PostReport { Id = 1 },
                new PostReport { Id = 2 },
                new PostReport { Id = 3 },
                new PostReport { Id = 4 },
            };

            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(reports);

            var manager = new ReportManager(dbContext);
            const int Id = 2;
            var res = manager.GetReportById(Id, (PostReport x) => x);

            Assert.That(res.Id, Is.EqualTo(Id));
        }

        [Test]
        public void GetReportById_Finding()
        {
            var reports = new List<FindingReport>
            {
                new FindingReport { Id = 1 },
                new FindingReport { Id = 2 },
                new FindingReport { Id = 3 },
                new FindingReport { Id = 4 },
            };

            var dbContext = Extensions.GetAppDbContext();
            dbContext.AddContent(reports);

            var manager = new ReportManager(dbContext);
            const int Id = 2;
            var res = manager.GetReportById(Id, (FindingReport x) => x);

            Assert.That(res.Id, Is.EqualTo(Id));
        }
    }
}
