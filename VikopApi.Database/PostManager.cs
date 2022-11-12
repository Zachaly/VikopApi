using Microsoft.EntityFrameworkCore;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Database
{
    [Implementation(typeof(IPostManager))]
    public class PostManager : IPostManager
    {
        private AppDbContext _dbContext;

        public PostManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddPost(Post post)
        {
            _dbContext.Posts.Add(post);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public IEnumerable<T> GetPosts<T>(int pageIndex, int pageSize, Func<Post, T> selector)
            => _dbContext.Posts
                .Include(post => post.Comment)
                .ThenInclude(comment => comment.Creator)
                .Include(post => post.Comment)
                .ThenInclude(comment => comment.Reactions)
                .Include(post => post.Tags)
                .ThenInclude(tag => tag.Tag)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(selector);

        public IEnumerable<T> GetTopPosts<T>(int pageIndex, int pageSize, Func<Post, T> selector)
            => _dbContext.Posts
                .Include(post => post.Comment)
                .ThenInclude(comment => comment.Creator)
                .Include(post => post.Comment)
                .ThenInclude(comment => comment.Reactions)
                .Include(post => post.Comment)
                .ThenInclude(comment => comment.SubComments)
                .ThenInclude(subcomment => subcomment.Comment)
                .ThenInclude(subcomment => subcomment.Reactions)
                .Include(post => post.Tags)
                .ThenInclude(tag => tag.Tag)
                .AsEnumerable()
                .OrderByDescending(post => post.Comment.CommentValue())
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(selector);

        public IEnumerable<T> GetNewPosts<T>(int pageIndex, int pageSize, Func<Post, T> selector)
            => _dbContext.Posts
                .Include(post => post.Comment)
                .ThenInclude(comment => comment.Creator)
                .Include(post => post.Comment)
                .ThenInclude(comment => comment.Reactions)
                .Include(post => post.Tags)
                .ThenInclude(tag => tag.Tag)
                .OrderByDescending(post => post.Comment.Created)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(selector);

        public int GetPageCount(int pageSize)
            => (int)Math.Ceiling(_dbContext.Posts.Count() / (decimal)pageSize);

        public IEnumerable<T> SearchPosts<T>(int pageIndex, int pageSize, IEnumerable<Func<Post, bool>> conditions, Func<Post, T> selector)
            => _dbContext.Posts
                    .Include(post => post.Comment)
                    .ThenInclude(comment => comment.Creator)
                    .Include(post => post.Comment)
                    .ThenInclude(comment => comment.Reactions)
                    .Include(post => post.Tags)
                    .ThenInclude(tag => tag.Tag)
                    .AsEnumerable()
                    .Where(post => conditions.All(condition => condition(post)))
                    .OrderByDescending(post => post.Comment.Created)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize)
                    .Select(selector);

        public async Task<bool> RemovePostById(int id)
        {
            var post = _dbContext.Posts.Include(post => post.Comment).FirstOrDefault(x => x.Id == id);

            _dbContext.Posts.Remove(post);
            _dbContext.Comments.Remove(post.Comment);

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
