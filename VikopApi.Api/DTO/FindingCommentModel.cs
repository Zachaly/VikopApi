namespace VikopApi.Api.DTO
{
    public class FindingCommentModel
    {
        public int FindingId { get; set; }
        public string Content { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
