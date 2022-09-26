namespace VikopApi.Api.DTO
{
    public class AddFindingModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
