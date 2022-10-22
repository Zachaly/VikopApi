using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikopApi.Application.Files.Abstractions
{
    public interface IFileService
    {
        string GetCommentPicture(int id);
        string GetFindingPicture(int id);
        string GetProfilePicture(string id);
    }
}
