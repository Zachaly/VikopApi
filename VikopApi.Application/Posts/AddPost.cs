using VikopApi.Application.Factories.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;
using VikopApi.Application.Tags.Abtractions;

namespace VikopApi.Application.Posts
{
    [Service]
    public class AddPost
    {
        private readonly ICommentManager _commentManager;
        private readonly IPostManager _postManager;
        private readonly ICommentFactory _commentFactory;
        private readonly IPostFactory _postFactory;
        private readonly ITagService _tagService;

        public AddPost(ICommentManager commentManager, IPostManager postManager, ICommentFactory commentFactory, IPostFactory postFactory, ITagService tagService)
        {
            _commentManager = commentManager;
            _postManager = postManager;
            _commentFactory = commentFactory;
            _postFactory = postFactory;
            _tagService = tagService;
        }

        public async Task<PostModel> Execute(AddPostRequest request)
        {
            var comment = _commentFactory.Create(request);

            var res = await _commentManager.AddComment(comment);

            if (!res)
                return null;

            var post = _postFactory.Create(comment);

            await _postManager.AddPost(post);

            return new PostModel
                {
                    Content = _commentManager.GetCommentById(comment.Id, comment => new CommentModel(comment)),
                    TagList = await _tagService.CreatePost(request.Tags, post.Id)
                };
        }
    }
}

