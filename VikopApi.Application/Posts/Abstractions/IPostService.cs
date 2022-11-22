using VikopApi.Application.Models;
using VikopApi.Application.Models.Post;
using VikopApi.Application.Models.Post.Requests;

namespace VikopApi.Application.Posts.Abstractions
{
    public interface IPostService
    {
        Task<PostModel> AddPost(AddPostRequest request);
        IEnumerable<PostModel> GetPosts(PagedRequest request);
        int GetPageCount(int pageSize);
        IEnumerable<PostModel> Search(SearchPostRequest request);
        Task<bool> RemovePostById(int id);
    }
}
