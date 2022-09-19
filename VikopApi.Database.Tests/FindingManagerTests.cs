using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions;
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

        private int SumReactions(IEnumerable<FindingReaction> reactions)
            => reactions.Sum(reaction => (int)reaction.Reaction);

        [Fact]
        public void Get_Finding_By_Id()
        {
            var finding = _findingManager.GetFindingById(1, x => x);

            Assert.Equal(1, finding.Id);
            Assert.Equal("finding1", finding.Title);
            Assert.Equal("user1", finding.Creator.UserName);
            Assert.Single(finding.Comments);
            Assert.Equal(3, finding.Reactions.Count());
            Assert.Equal(1, SumReactions(finding.Reactions));
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

        [Fact]
        public async Task Add_Reaction()
        {
            var reaction = new FindingReaction
            {
                FindingId = 3,
                UserId = "2",
                Reaction = Reaction.Positive
            };

            var res = await _findingManager.AddReaction(reaction);

            var finding = _dbContext.Findings.Include(finding => finding.Reactions)
                .FirstOrDefault(finding => finding.Id == 3);

            Assert.True(res);
            Assert.Equal(3, finding.Reactions.Count());
            Assert.Equal(3, SumReactions(finding.Reactions));
        }

        [Fact]
        public async Task Add_Existent_Reaction()
        {
            var reaction = new FindingReaction
            {
                FindingId = 2,
                UserId = "2",
                Reaction = Reaction.Positive
            };

            var res = await _findingManager.AddReaction(reaction);

            var finding = _dbContext.Findings.Include(finding => finding.Reactions)
                .FirstOrDefault(finding => finding.Id == 2);

            Assert.False(res);
            Assert.Equal(2, finding.Reactions.Count());
            Assert.Equal(-2, SumReactions(finding.Reactions));
        }

        [Fact]
        public async Task Change_Reaction()
        {
            var reaction = new FindingReaction
            {
                FindingId = 3,
                UserId = "1",
                Reaction = Reaction.Negative
            };

            var res = await _findingManager.ChangeReaction(reaction);

            var finding = _dbContext.Findings.Include(finding => finding.Reactions)
                .FirstOrDefault(finding => finding.Id == 3);

            Assert.True(res);
            Assert.Equal(2, finding.Reactions.Count());
            Assert.Equal(0, SumReactions(finding.Reactions));
        }

        [Fact]
        public async Task Change_Nonexistent_Reaction()
        {
            var reaction = new FindingReaction
            {
                FindingId = 3,
                UserId = "2",
                Reaction = Reaction.Negative
            };

            var res = await _findingManager.ChangeReaction(reaction);

            var finding = _dbContext.Findings.Include(finding => finding.Reactions)
                .FirstOrDefault(finding => finding.Id == 3);

            Assert.False(res);
            Assert.Equal(2, finding.Reactions.Count());
            Assert.Equal(2, SumReactions(finding.Reactions));
        }

        [Fact]
        public async Task Delete_Reaction()
        {
            var res = await _findingManager.DeleteReaction(2, "3");

            var finding = _dbContext.Findings.Include(finding => finding.Reactions)
                .FirstOrDefault(finding => finding.Id == 2);

            Assert.True(res);
            Assert.Single(finding.Reactions);
            Assert.Equal(-1, SumReactions(finding.Reactions));
        }

        [Fact]
        public async Task Delete_Nonexistent_Reaction()
        {
            var res = await _findingManager.DeleteReaction(2, "1");

            var finding = _dbContext.Findings.Include(finding => finding.Reactions)
                .FirstOrDefault(finding => finding.Id == 2);

            Assert.True(res);
            Assert.Equal(2, finding.Reactions.Count());
            Assert.Equal(-2, SumReactions(finding.Reactions));
        }

        [Fact]
        public void Get_Nonexistent_User_Reaction()
        {
            var res = _findingManager.GetUserReaction(2, "1", reaction => (int?)reaction.Reaction ?? 0);

            Assert.Equal(0, res);
        }

        [Fact]
        public void Get_User_Reaction()
        {
            var res = _findingManager.GetUserReaction(1, "1", reaction => (int?)reaction.Reaction ?? 0);

            Assert.Equal(1, res);
        }
    }
}
