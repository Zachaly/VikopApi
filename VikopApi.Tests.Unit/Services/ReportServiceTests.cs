using Moq;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Reports;
using VikopApi.Application.Reports.Abstractions;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Tests.Unit.Services
{
    [TestFixture]
    public class ReportServiceTests
    {
        [Test]
        [TestCase(0, 2)]
        [TestCase(2, 1)]
        [TestCase(null, 1)]
        [TestCase(0, null)]
        public void GetFindingReports(int? pageIndex, int? pageSize)
        {
            var reports = new List<FindingReport>
            {
                new FindingReport { Id = 1 },
                new FindingReport { Id = 2 },
                new FindingReport { Id = 3 },
                new FindingReport { Id = 4 },
            };

            var managerMock = new Mock<IReportManager>();
            managerMock.Setup(x => x.GetReports(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<FindingReport, ReportListItem>>()))
                .Returns((int index, int size, Func<FindingReport, ReportListItem> selector)
                    => reports.Skip(index * size).Take(size).Select(selector));
            

            var factoryMock = new Mock<IReportFactory>();
            factoryMock.Setup(x => x.CreateListItem(It.IsAny<FindingReport>()))
                .Returns((FindingReport report) => new ReportListItem { Id = report.Id });

            var service = new ReportService(managerMock.Object, factoryMock.Object);

            var res = service.GetFindingReports(pageIndex, pageSize);

            var expected = reports.Skip((pageIndex ?? 0) * (pageSize ?? 10)).Take(pageSize ?? 10);

            Assert.Multiple(() =>
            {
                Assert.That(res.Count(), Is.EqualTo(expected.Count()));
                Assert.That(res.All(x => reports.Any(y => x.Id == y.Id)));
            });
        }

        [Test]
        [TestCase(0, 2)]
        [TestCase(2, 1)]
        [TestCase(null, 1)]
        [TestCase(0, null)]
        public void GetPostReports(int? pageIndex, int? pageSize)
        {
            var reports = new List<PostReport>
            {
                new PostReport { Id = 1 },
                new PostReport { Id = 2 },
                new PostReport { Id = 3 },
                new PostReport { Id = 4 },
            };

            var managerMock = new Mock<IReportManager>();
            managerMock.Setup(x => x.GetReports(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<PostReport, ReportListItem>>()))
                .Returns((int index, int size, Func<PostReport, ReportListItem> selector)
                    => reports.Skip(index * size).Take(size).Select(selector));

            var factoryMock = new Mock<IReportFactory>();
            factoryMock.Setup(x => x.CreateListItem(It.IsAny<PostReport>()))
                .Returns((PostReport report) => new ReportListItem { Id = report.Id });

            var service = new ReportService(managerMock.Object, factoryMock.Object);

            var res = service.GetPostReports(pageIndex, pageSize);

            var expected = reports.Skip((pageIndex ?? 0) * (pageSize ?? 10)).Take(pageSize ?? 10);

            Assert.Multiple(() =>
            {
                Assert.That(res.Count(), Is.EqualTo(expected.Count()));
                Assert.That(res.All(x => reports.Any(y => x.Id == y.Id)));
            });
        }

        [Test]
        public void GetFindingReportById()
        {
            var reports = new List<FindingReport>
            {
                new FindingReport { Id = 1 },
                new FindingReport { Id = 2 },
                new FindingReport { Id = 3 },
                new FindingReport { Id = 4 },
            };

            var managerMock = new Mock<IReportManager>();
            managerMock.Setup(x => x.GetReportById(It.IsAny<int>(), It.IsAny<Func<FindingReport, FindingReportModel>>()))
                .Returns((int id, Func<FindingReport, FindingReportModel> selector)
                    => reports.Where(x => x.Id == id).Select(selector).FirstOrDefault());

            var factoryMock = new Mock<IReportFactory>();
            factoryMock.Setup(x => x.CreateModel(It.IsAny<FindingReport>()))
                .Returns((FindingReport report) => new FindingReportModel { Id = report.Id });

            var service = new ReportService(managerMock.Object, factoryMock.Object);
            const int Id = 3;
            var res = service.GetFindingReportById(Id);

            Assert.That(res.Id, Is.EqualTo(Id));
        }

        [Test]
        public void GetPostReportById()
        {
            var reports = new List<PostReport>
            {
                new PostReport { Id = 1 },
                new PostReport { Id = 2 },
                new PostReport { Id = 3 },
                new PostReport { Id = 4 },
            };

            var managerMock = new Mock<IReportManager>();
            managerMock.Setup(x => x.GetReportById(It.IsAny<int>(), It.IsAny<Func<PostReport, PostReportModel>>()))
                .Returns((int id, Func<PostReport, PostReportModel> selector)
                    => reports.Where(x => x.Id == id).Select(selector).FirstOrDefault());

            var factoryMock = new Mock<IReportFactory>();
            factoryMock.Setup(x => x.CreateModel(It.IsAny<PostReport>()))
                .Returns((PostReport report) => new PostReportModel { Id = report.Id });

            var service = new ReportService(managerMock.Object, factoryMock.Object);
            const int Id = 3;
            var res = service.GetPostReportById(Id);

            Assert.That(res.Id, Is.EqualTo(Id));
        }

        [Test]
        public async Task AddFindingReport()
        {
            var reports = new List<FindingReport>();
            var managerMock = new Mock<IReportManager>();
            managerMock.Setup(x => x.AddReport(It.IsAny<FindingReport>()))
                .Callback((FindingReport report) => reports.Add(report))
                .ReturnsAsync(true);

            var factoryMock = new Mock<IReportFactory>();
            factoryMock.Setup(x => x.CreateFinding(It.IsAny<AddReportRequest>()))
                .Returns((AddReportRequest request) => new FindingReport
                {
                    FindingId = request.ObjectId,
                    ReportingUserId = request.ReportingUserId,
                    Reason = request.Reason
                });

            var service = new ReportService(managerMock.Object, factoryMock.Object);

            var request = new AddReportRequest
            {
                ObjectId = 1,
                Reason = "reason",
                ReportingUserId = "id"
            };

            var res = await service.AddFindingReport(request);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(reports.Any(x => x.Reason == request.Reason && x.ReportingUserId == request.ReportingUserId && x.FindingId == request.ObjectId));
            });
        }

        [Test]
        public async Task AddPostReport()
        {
            var reports = new List<PostReport>();
            var managerMock = new Mock<IReportManager>();
            managerMock.Setup(x => x.AddReport(It.IsAny<PostReport>()))
                .Callback((PostReport report) => reports.Add(report))
                .ReturnsAsync(true);

            var factoryMock = new Mock<IReportFactory>();
            factoryMock.Setup(x => x.CreatePost(It.IsAny<AddReportRequest>()))
                .Returns((AddReportRequest request) => new PostReport
                {
                    PostId = request.ObjectId,
                    ReportingUserId = request.ReportingUserId,
                    Reason = request.Reason
                });

            var service = new ReportService(managerMock.Object, factoryMock.Object);

            var request = new AddReportRequest
            {
                ObjectId = 1,
                Reason = "reason",
                ReportingUserId = "id"
            };

            var res = await service.AddPostReport(request);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(reports.Any(x => x.Reason == request.Reason && x.ReportingUserId == request.ReportingUserId && x.PostId == request.ObjectId));
            });
        }

        [Test]
        public async Task RemoveFindingReport_Success()
        {
            var reports = new List<FindingReport>
            {
                new FindingReport { Id = 1 },
                new FindingReport { Id = 2 },
                new FindingReport { Id = 3 },
                new FindingReport { Id = 4 },
            };
            var managerMock = new Mock<IReportManager>();
            managerMock.Setup(x => x.RemoveFindingReport(It.IsAny<int>()))
                .Callback((int id) => reports.Remove(reports.FirstOrDefault(x => x.Id == id)))
                .ReturnsAsync(true);

            var factoryMock = new Mock<IReportFactory>();

            var service = new ReportService(managerMock.Object, factoryMock.Object);

            const int Id = 3;
            var res = await service.RemoveFindingReport(Id);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(!reports.Any(x => x.Id == Id));
            });
        }

        [Test]
        public async Task RemoveFindingReport_Fail()
        {
            var reports = new List<FindingReport>
            {
                new FindingReport { Id = 1 },
                new FindingReport { Id = 2 },
                new FindingReport { Id = 3 },
                new FindingReport { Id = 4 },
            };
            var managerMock = new Mock<IReportManager>();
            managerMock.Setup(x => x.RemoveFindingReport(It.IsAny<int>()))
                .Callback((int id) => throw new Exception())
                .ReturnsAsync(true);

            var factoryMock = new Mock<IReportFactory>();

            var service = new ReportService(managerMock.Object, factoryMock.Object);

            const int Id = 3;
            var res = await service.RemoveFindingReport(Id);

            Assert.That(res, Is.False);
        }

        [Test]
        public async Task RemovePostReport_Success()
        {
            var reports = new List<PostReport>
            {
                new PostReport { Id = 1 },
                new PostReport { Id = 2 },
                new PostReport { Id = 3 },
                new PostReport { Id = 4 },
            };
            var managerMock = new Mock<IReportManager>();
            managerMock.Setup(x => x.RemovePostReport(It.IsAny<int>()))
                .Callback((int id) => reports.Remove(reports.FirstOrDefault(x => x.Id == id)))
                .ReturnsAsync(true);

            var factoryMock = new Mock<IReportFactory>();

            var service = new ReportService(managerMock.Object, factoryMock.Object);

            const int Id = 3;
            var res = await service.RemovePostReport(Id);

            Assert.Multiple(() =>
            {
                Assert.That(res, Is.True);
                Assert.That(!reports.Any(x => x.Id == Id));
            });
        }

        [Test]
        public async Task RemovePostReport_Fail()
        {
            var reports = new List<PostReport>
            {
                new PostReport { Id = 1 },
                new PostReport { Id = 2 },
                new PostReport { Id = 3 },
                new PostReport { Id = 4 },
            };
            var managerMock = new Mock<IReportManager>();
            managerMock.Setup(x => x.RemovePostReport(It.IsAny<int>()))
                .Callback((int id) => throw new Exception())
                .ReturnsAsync(true);

            var factoryMock = new Mock<IReportFactory>();

            var service = new ReportService(managerMock.Object, factoryMock.Object);

            const int Id = 3;
            var res = await service.RemovePostReport(Id);

            Assert.That(res, Is.False);
        }
    }
}
