using VikopApi.Application.Models.Post;
using VikopApi.Application.Models.Post.Requests;
using VikopApi.Domain.Enums;

namespace VikopApi.Application.Posts.Abstractions
{
    public interface IPostService
    {
        Task<PostModel> AddPost(AddPostRequest request);
        IEnumerable<PostModel> GetPosts(SortingType? sortingType, int? pageIndex, int? pageSize);
        int GetPageCount(int pageSize);
        IEnumerable<PostModel> Search(SearchPostRequest request);
        Task<bool> RemovePostById(int id);
    }
}
