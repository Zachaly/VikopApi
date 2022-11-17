namespace VikopApi.Application.Models.Finding.Requests
{
    public class AddFindingRequest
    {
        public string Title { get; set; }
        public string CreatorId { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Picture { get; set; }
        public IEnumerable<string> TagList { get; set; }
    }
}
