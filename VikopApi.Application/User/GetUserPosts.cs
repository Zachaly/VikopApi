using VikopApi.Application.Models;

namespace VikopApi.Application.User
{
    [Service]
    public class GetUserPosts
    {
        private readonly IApplicationUserManager _appUserManager;

        public GetUserPosts(IApplicationUserManager applicationUserManager)
        {
            _appUserManager = applicationUserManager;
        }

        public IEnumerable<CommentModel> Execute(string id)
            => _appUserManager.GetUserPosts(id, post => new CommentModel(post.Comment));
    }
}
