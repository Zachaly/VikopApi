namespace VikopApi.Application.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public int CreatorRank { get; set; }
        public string Content { get; set; }
        public string Created { get; set; }
        public int Reactions { get; set; }
        public bool HasPicture { get; set; }

        public CommentModel()
        {

        }
    }
}
