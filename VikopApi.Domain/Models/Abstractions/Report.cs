namespace VikopApi.Domain.Models.Abstractions
{
    public abstract class Report
    {
        public int Id { get; set; }
        public string? ReportingUserId { get; set; }
        public ApplicationUser? ReportingUser { get; set; }
        public string? Reason { get; set; }
        public DateTime Created { get; set; }
    }
}
