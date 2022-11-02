using VikopApi.Domain.Models.Abstractions;

namespace VikopApi.Application
{
    public static class Extensions
    {
        public static string GetTime(this DateTime @this)
            => @this.ToString("yyyy-M-dd, HH:m");

        public static string GetDate(this DateTime @this)
            => @this.ToString("yyyy-M-dd");

        public static int SumReactions(this IEnumerable<IReaction> @this)
            => @this.Sum(reaction => (int)reaction.Reaction);
    }
}
