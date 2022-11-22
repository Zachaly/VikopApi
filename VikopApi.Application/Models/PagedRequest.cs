using VikopApi.Domain.Enums;

namespace VikopApi.Application.Models
{
    public class PagedRequest
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public SortingType? SortingType { get; set; }

        public (int index, int size) GetIndexAndSize() => (PageIndex ?? 0, PageSize ?? 10);
    }
}
