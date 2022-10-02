namespace VikopApi.Application.HelperModels
{
    public class CommentRequest
    {
        public string CreatorId { get; set; }
        public string Content { get; set; }
        public string? Picture { get; set; }
    }
}
