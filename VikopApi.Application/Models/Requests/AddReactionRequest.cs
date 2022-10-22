using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopApi.Domain.Enums;

namespace VikopApi.Application.Models.Requests
{
    public class AddReactionRequest
    {
        public int ObjectId { get; set; }
        public string UserId { get; set; }
        public Reaction Reaction { get; set; }
    }
}
