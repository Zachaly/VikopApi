namespace VikopApi.Application.Models.Finding.Requests
{
    public class SearchFindingsRequest
    {
        public string Text { get; set; }
        public bool? SearchTitle { get; set; }
        public bool? SearchTag { get; set; }
        public bool? SearchCreator { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
