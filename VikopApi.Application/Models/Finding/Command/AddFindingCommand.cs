using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Models.Command;

namespace VikopApi.Application.Models.Finding.Command
{
    public class AddFindingCommand : IRequest<CommandResponseModel>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public IFormFile? Picture { get; set; }
        public string Tags { get; set; }
    }
}
