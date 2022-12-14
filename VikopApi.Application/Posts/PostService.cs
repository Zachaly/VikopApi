using VikopApi.Application.Comments.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Post;
using VikopApi.Application.Models.Post.Requests;
using VikopApi.Application.Posts.Abstractions;
using VikopApi.Application.Tags.Abtractions;
using VikopApi.Domain.Enums;

namespace VikopApi.Application.Posts
{
    [Implementation(typeof(IPostService))]
    public class PostService : IPostService
    {
        private readonly IPostFactory _postFactory;
        private readonly IPostManager _postManager;
        private readonly ICommentFactory _commentFactory;
        private readonly ICommentManager _commentManager;
        private readonly ITagService _tagService;

        public PostService(IPostFactory postFactory,
            IPostManager postManager,
            ICommentFactory commentFactory,
            ICommentManager commentManager,
            ITagService tagService)
        {
            _postFactory = postFactory;
            _postManager = postManager;
            _commentFactory = commentFactory;
            _commentManager = commentManager;
            _tagService = tagService;
        }

        public async Task<PostModel> AddPost(AddPostRequest request)
        {
            var comment = _commentFactory.Create(request);

            var res = await _commentManager.AddComment(comment);

            if (!res)
                return null;

            var post = _postFactory.Create(comment);

            await _postManager.AddPost(post);

            return _postFactory.CreateModel(_commentManager.GetCommentById(comment.Id, x => x), await _tagService.CreatePost(request.Tags, post.Id));
        }

        public int GetPageCount(int pageSize)
            => _postManager.GetPageCount(pageSize);

        public IEnumerable<PostModel> GetPosts(PagedRequest request)
        {
            Func<Post, PostModel> postSelector = (post) => _postFactory.CreateModel(post);

            var (index, size) = request.GetIndexAndSize();

            if (request.SortingType.HasValue)
            {
                if(request.SortingType.Value == SortingType.New)
                {
                    return _postManager.GetNewPosts(index, size, postSelector);
                }
                else if(request.SortingType.Value == SortingType.Top)
                {
                    return _postManager.GetTopPosts(index, size, postSelector);
                }
            }

            return _postManager.GetPosts(index, size, postSelector);
        }

        public IEnumerable<PostModel> Search(SearchPostRequest request)
        {
            var conditions = new List<Func<Post, bool>>();

            var (index, size) = request.GetIndexAndSize();

            if (request.SearchCreator.GetValueOrDefault())
                conditions.Add(post => post.Comment.Creator.UserName.Contains(request.Text));
            if (request.SearchTag.GetValueOrDefault())
                conditions.Add(post => post.Tags.Any(tag => tag.Tag.Name.Contains(request.Text)));

            return _postManager.SearchPosts(index, size, conditions, post => _postFactory.CreateModel(post));
        }

        public Task<bool> RemovePostById(int id)
            => _postManager.RemovePostById(id);
    }
}
