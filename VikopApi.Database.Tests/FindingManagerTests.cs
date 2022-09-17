using Microsoft.EntityFrameworkCore;
using NuGet.Frameworks;
using VikopApi.Domain.Infractructure;

namespace VikopApi.Database.Tests
{
    public class FindingManagerTests : DatabaseTest
    {
        private readonly IFindingManager _findingManager;

        public FindingManagerTests() : base()
        {
            _findingManager = new FindingManager(_dbContext);
        }

        [Fact]
        public void Get_Findings()
        {
            var findings = _findingManager.GetFindings(x => x);

            Assert.Equal(3, findings.Count());
            Assert.Contains(findings, finding => finding.Id == 1 && finding.Creator.UserName == "user1");
            Assert.Contains(findings, finding => finding.Id == 3 && finding.Comments.Count() == 3);
        }

        [Fact]
        public void Get_Finding_By_Id()
        {
            var finding = _findingManager.GetFindingById(1, x => x);

            Assert.Equal(1, finding.Id);
            Assert.Equal("finding1", finding.Title);
            Assert.Equal("user1", finding.Creator.UserName);
            Assert.Single(finding.Comments);
        }

        [Fact]
        public void Get_Finding_By_Nonexistent_Id()
        {
            var finding = _findingManager.GetFindingById(69, x => x);

            Assert.Null(finding);
        }

        [Fact]
        public async Task Add_Finding()
        {
            var finding = new Finding
            {
                Id = 4,
                Created = new DateTime(2137, 6, 9),
                CreatorId = "3",
                Description = "new finding",
                Link = "https://link4.com",
                Picture = "placeholder.jpg",
                Title = "finding4"
            };

            var res = await _findingManager.AddFinding(finding);

            var user = _dbContext.Users.Include(user => user.Findings).FirstOrDefault(x => x.Id == "3");

            Assert.True(res);
            Assert.Contains(_dbContext.Findings, finding => finding.Id == 4);
            Assert.Contains(_dbContext.Findings, finding => finding.Title == "finding4");
            Assert.Contains(user?.Findings, finding => finding.Id == 4);
        }

        [Fact]
        public async Task Add_Invalid_Finding_Error()
        {
            var finding = new Finding();

            await Assert.ThrowsAsync<DbUpdateException>(async () => await _findingManager.AddFinding(finding));
        }
    }
}
