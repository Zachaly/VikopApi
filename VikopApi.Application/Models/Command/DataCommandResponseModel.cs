namespace VikopApi.Application.Models.Command
{
    public class DataCommandResponseModel<T> : CommandResponseModel
    {
        public T? Data { get; set; }
    }
}
