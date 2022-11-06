namespace VikopApi.Application.Models.Requests
{
    public class SearchPostRequest
    {
        public string Text { get; set; }
        public bool? SearchTag { get; set; }
        public bool? SearchCreator { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
