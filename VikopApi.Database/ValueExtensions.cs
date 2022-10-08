using VikopApi.Domain.Models;

namespace VikopApi.Database
{
    internal static class ValueExtensions
    {
        public static int FindingValue(this Finding @this)
        {
            int value = 0;

            value += @this.Reactions.ReactionScore();

            value += @this.Comments.Count() * 2;

            @this.Comments
                .Select(comment => comment.Comment).ToList()
                .ForEach(comment => value += comment.CommentValue() / 100);

            var timeFromCreation = DateTime.Now - @this.Created;

            value -= timeFromCreation.Days * 1000;

            return value;
        }

        public static int CommentValue(this Comment @this)
        {
            int value = 0;

            value += @this.Reactions.ReactionScore();
            value += @this.SubComments.Count() * 2;

            @this.SubComments
                .Select(subcomment => subcomment.Comment).ToList()
                .ForEach(subcomment => value += subcomment.Reactions.ReactionScore());

            var timeFromCreation = DateTime.Now - @this.Created;

            value -= timeFromCreation.Days * 1000;

            return value;
        }

        private static int ReactionScore(this IEnumerable<IReaction> @this)
            => @this.Sum(reaction => (int)reaction.Reaction) * 5 + @this.Count() * 2;
    }
}
