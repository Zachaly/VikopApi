namespace VikopApi.Application.Models
{
    public class DataCommandResponseModel<T> : CommandResponseModel
    {
        public T? Data { get; set; }
    }
}
