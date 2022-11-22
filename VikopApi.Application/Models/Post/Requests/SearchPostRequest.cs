namespace VikopApi.Application.Models.Post.Requests
{
    public class SearchPostRequest : PagedRequest
    {
        public string Text { get; set; }
        public bool? SearchTag { get; set; }
        public bool? SearchCreator { get; set; }
    }
}
