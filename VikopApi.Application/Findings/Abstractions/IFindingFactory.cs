using VikopApi.Application.Models.Finding;
using VikopApi.Application.Models.Finding.Requests;

namespace VikopApi.Application.Findings.Abstractions
{
    public interface IFindingFactory
    {
        public FindingListItemModel CreateListItem(Finding finding);
        public FindingModel CreateModel(Finding finding);
        public Finding Create(AddFindingRequest request);
    }
}
