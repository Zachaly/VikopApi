using VikopApi.Domain.Enums;

namespace VikopApi.Application.Findings
{
    [Service]
    public class AddReaction
    {
        private readonly IFindingManager _findingManager;

        public AddReaction(IFindingManager findingManager)
        {
            _findingManager = findingManager;
        }

        public async Task<bool> Execute(Request request)
        {
            var reaction = new FindingReaction
            {
                FindingId = request.FindingId,
                UserId = request.UserId,
                Reaction = request.Reaction
            };

            return await _findingManager.AddReaction(reaction);
        } 

        public class Request
        {
            public string UserId { get; set; }
            public int FindingId { get; set; }
            public Reaction Reaction { get; set; }
        }
    }
}
