using VikopApi.Application.Models.User;

namespace VikopApi.Application.Models.Report
{
    public class ReportListItem
    {
        public int Id { get; set; }
        public UserListItemModel ReportingUser { get; set; }
        public int ObjectId { get; set; }
        public string Created { get; set; }
    }
}
