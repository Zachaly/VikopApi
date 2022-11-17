namespace VikopApi.Application.Models.Report.Requests
{
    public class AddReportRequest
    {
        public string ReportingUserId { get; set; }
        public int ObjectId { get; set; }
        public string Reason { get; set; }
    }
}
