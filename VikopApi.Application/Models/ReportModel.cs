using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikopApi.Application.Models
{
    public abstract class ReportModel
    {
        public int Id { get; set; }
        public UserListItemModel ReportingUser { get; set; }
        public string Reason { get; set; }
        public string Created { get; set; }
    }

    public class PostReportModel : ReportModel
    {
        public PostModel Post { get; set; }
    }

    public class FindingReportModel : ReportModel
    {
        public FindingListItemModel Finding { get; set; }
    }
}
