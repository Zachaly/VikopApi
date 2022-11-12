namespace VikopApi.Application.Models
{
    public class ReportListItem
    {
        public int Id { get; set; }
        public UserListItemModel ReportingUser { get; set; }
        public int ObjectId { get; set; }
        public string Created { get; set; }
    }
}
