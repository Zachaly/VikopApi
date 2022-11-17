using VikopApi.Application.Models.Command;

namespace VikopApi.Application.Command.Abstractions
{
    public interface ICommandResponseFactory
    {
        public CommandResponseModel CreateSuccess();
        public CommandResponseModel CreateFailure(IDictionary<string, IEnumerable<string>> errors);
        public DataCommandResponseModel<T> CreateSuccess<T>(T data);
        public DataCommandResponseModel<T> CreateFailure<T>(IDictionary<string, IEnumerable<string>> errors);
    }
}
