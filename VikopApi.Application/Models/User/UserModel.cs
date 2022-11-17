using VikopApi.Domain.Enums;

namespace VikopApi.Application.Models.User
{
    public class UserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Created { get; set; }
        public Rank Rank { get; set; }
    }
}
