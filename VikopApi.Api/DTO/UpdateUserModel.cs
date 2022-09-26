namespace VikopApi.Api.DTO
{
    public class UpdateUserModel
    {
        public string Username { get; set; }
        public IFormFile? ProfilePicture { get; set; }
    }
}
