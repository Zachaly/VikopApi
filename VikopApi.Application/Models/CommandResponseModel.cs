namespace VikopApi.Application.Models
{
    public class CommandResponseModel
    {
        public CommandResponseCode Code { get; set; }
        public IDictionary<string, IEnumerable<string>>? Errors { get; set; }
    }

    public enum CommandResponseCode
    {
        Success = 1,
        Fail = -1
    }
}
