namespace VikopApi.Application.Posts
{
    [Service]
    public class GetPageCount
    {
        private IPostManager _postManager;

        public GetPageCount(IPostManager postManager)
        {
            _postManager = postManager;
        }

        public int Execute(int pageSize) => _postManager.GetPageCount(pageSize);
    }
}
