using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikopApi.Application.Models.Requests
{
    public class AddFindingCommentRequest : AddCommentRequest
    {
        public int FindingId { get; set; }
    }
}
