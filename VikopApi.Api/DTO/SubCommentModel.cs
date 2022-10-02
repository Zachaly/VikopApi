namespace VikopApi.Api.DTO
{
    public class SubCommentModel
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
