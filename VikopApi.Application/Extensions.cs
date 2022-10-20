using VikopApi.Domain.Models.Abstractions;

namespace VikopApi.Application
{
    internal static class Extensions
    {
        public static string GetTime(this DateTime @this)
            => @this.ToString("yyyy-M-dd, HH:m");

        public static int SumReactions(this IEnumerable<IReaction> @this)
            => @this.Sum(reaction => (int)reaction.Reaction);
    }
}
