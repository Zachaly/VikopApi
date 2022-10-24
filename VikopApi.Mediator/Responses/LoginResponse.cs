
namespace VikopApi.Mediator.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; } = "";
        public bool Error { get; set; } = false;
        public string[] Errors { get; set; }
    }
}
