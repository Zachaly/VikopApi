namespace VikopApi.Application.Models.Reaction.Requests
{
    public class AddReactionRequest
    {
        public int ObjectId { get; set; }
        public string UserId { get; set; }
        public Domain.Enums.Reaction Reaction { get; set; }
    }
}
