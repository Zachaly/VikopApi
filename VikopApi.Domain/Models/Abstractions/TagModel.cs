namespace VikopApi.Domain.Models.Abstractions
{
    public abstract class TagModel
    {
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
