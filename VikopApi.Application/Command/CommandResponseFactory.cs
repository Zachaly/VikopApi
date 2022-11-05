using VikopApi.Application.Command.Abstractions;
using VikopApi.Application.Models;

namespace VikopApi.Application.Command
{
    [Implementation(typeof(ICommandResponseFactory))]
    public class CommandResponseFactory : ICommandResponseFactory
    {

        public CommandResponseModel CreateFailure(IDictionary<string, IEnumerable<string>> errors)
            => new CommandResponseModel
            {
                Code = CommandResponseCode.Fail,
                Errors = errors
            };

        public DataCommandResponseModel<T> CreateFailure<T>(IDictionary<string, IEnumerable<string>> errors)
            => new DataCommandResponseModel<T>
            {
                Code = CommandResponseCode.Fail,
                Errors = errors,
            };

        public CommandResponseModel CreateSuccess()
            => new CommandResponseModel
            {
                Code = CommandResponseCode.Success,
                Errors = null
            };

        public DataCommandResponseModel<T> CreateSuccess<T>(T data)
            => new DataCommandResponseModel<T>
            {
                Code = CommandResponseCode.Success,
                Data = data,
                Errors = null
            };
    }
}
