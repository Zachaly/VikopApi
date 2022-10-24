using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Models;

namespace VikopApi.Mediator.Requests
{
    public class AddFindingQuery : IRequest<Unit>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public IFormFile? Picture { get; set; }
        public string Tags { get; set; }
    }
}
