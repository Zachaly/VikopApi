namespace VikopApi.Application.HelperModels
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string Content { get; set; }
        public string Created { get; set; }
        public int Reactions { get; set; }
        public bool HasPicture { get; set; }

        public CommentModel()
        {

        }

        public CommentModel(Comment comment)
        {
            Content = comment.Content;
            Created = comment.Created.GetTime();
            CreatorId = comment.CreatorId;
            CreatorName = comment.Creator.UserName;
            Id = comment.Id;
            Reactions = comment.Reactions.SumReactions();
            HasPicture = !string.IsNullOrEmpty(comment.Picture);
        }
    }
}
