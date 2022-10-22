namespace VikopApi.Application.Models.Requests
{
    public class AddCommentRequest
    {
        public string CreatorId { get; set; }
        public string Content { get; set; }
        public string? Picture { get; set; }
    }
}
