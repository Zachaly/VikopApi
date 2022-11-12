using VikopApi.Domain.Models.Abstractions;

namespace VikopApi.Domain.Models
{
    public class FindingReport : Report
    {
        public int? FindingId { get; set; }
        public Finding? Finding { get; set; }
    }
}
