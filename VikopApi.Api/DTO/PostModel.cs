namespace VikopApi.Api.DTO
{
    public class PostModel
    {
        public string Content { get; set; }
        public IFormFile? Picture { get; set; }
        public string Tags { get; set; }
    }
}
