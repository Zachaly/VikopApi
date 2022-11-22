namespace VikopApi.Application.Models.Finding.Requests
{
    public class SearchFindingsRequest : PagedRequest
    {
        public string Text { get; set; }
        public bool? SearchTitle { get; set; }
        public bool? SearchTag { get; set; }
        public bool? SearchCreator { get; set; }
    }
}
