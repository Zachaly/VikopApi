using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopApi.Database.Migrations;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;
using VikopApi.Domain.Models.Abstractions;

namespace VikopApi.Database
{
    [Implementation(typeof(IReactionManager))]
    public class ReactionManager : IReactionManager
    {
        private AppDbContext _dbContext;

        public ReactionManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private CommentReaction GetCommentReaction(int commentId, string userId)
            => _dbContext.CommentReactions
                .FirstOrDefault(reaction => reaction.CommentId == commentId && reaction.UserId == userId);

        private FindingReaction GetFindingReaction(int findingId, string userId)
            => _dbContext.FindingReactions
                .FirstOrDefault(reaction => reaction.FindingId == findingId && reaction.UserId == userId);

        public async Task<bool> AddReaction(CommentReaction reaction)
        {
            if (GetCommentReaction(reaction.CommentId, reaction.UserId) != null)
            {
                return false;
            }

            _dbContext.CommentReactions.Add(reaction);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddReaction(FindingReaction reaction)
        {
            if (GetFindingReaction(reaction.FindingId, reaction.UserId) != null)
            {
                return false;
            }

            _dbContext.FindingReactions.Add(reaction);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangeReaction(FindingReaction reaction)
        {
            var originalReaction = GetFindingReaction(reaction.FindingId, reaction.UserId);

            if (originalReaction is null)
            {
                return false;
            }

            originalReaction.Reaction = reaction.Reaction;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangeReaction(CommentReaction reaction)
        {
            var originalReaction = GetCommentReaction(reaction.CommentId, reaction.UserId);

            if (originalReaction is null)
            {
                return false;
            }

            originalReaction.Reaction = reaction.Reaction;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public T GetUserCommentReaction<T>(string userId, int commentId, Func<CommentReaction, T> selector)
            => _dbContext.CommentReactions
                    .Where(reaction => reaction.CommentId == commentId && reaction.UserId == userId)
                    .Select(selector)
                    .FirstOrDefault();

        public T GetUserFindingReaction<T>(string userId, int findingId, Func<FindingReaction, T> selector)
            => _dbContext.FindingReactions
                    .Where(reaction => reaction.UserId == userId && reaction.FindingId == findingId)
                    .Select(selector)
                    .FirstOrDefault();

        public async Task<bool> DeleteCommentReaction(int commentId, string userId)
        {
            var reaction = GetCommentReaction(commentId, userId);

            if (reaction is null)
            {
                return true;
            }

            _dbContext.CommentReactions.Remove(reaction);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteFindingReaction(int findingId, string userId)
        {
            var reaction = GetFindingReaction(findingId, userId);

            if (reaction is null)
            {
                return true;
            }

            _dbContext.FindingReactions.Remove(reaction);

            return await _dbContext.SaveChangesAsync() > 0;
        }
    }
}
