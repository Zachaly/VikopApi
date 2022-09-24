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

        public IEnumerable<PostModel> Execute(string id)
            => _appUserManager.GetUserPosts(id, post => new PostModel
            {
                Id = post.Comment.Id,
                Content = post.Comment.Content,
                Created = post.Comment.Created.GetTime(),
                CreatorId = post.Comment.CreatorId,
                CreatorName = post.Comment.Creator.UserName,
                Reactions = post.Comment.Reactions.SumReactions()
            });

        public class PostModel
        {
            public int Id { get; set; }
            public string CreatorId { get; set; }
            public string CreatorName { get; set; }
            public string Content { get; set; }
            public string Created { get; set; }
            public int Reactions { get; set; }
        }
    }
}
