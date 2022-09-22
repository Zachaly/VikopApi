﻿using Microsoft.EntityFrameworkCore;
using VikopApi.Domain.Infractructure;
using VikopApi.Domain.Models;

namespace VikopApi.Database
{
    public class CommentManager : ICommentManager
    {
        private readonly AppDbContext _dbContext;

        public CommentManager(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddComment(Comment comment)
        {
            _dbContext.Comments.Add(comment);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddFindingComment(int commentId, int findingId)
        {
            if(!_dbContext.Findings.Any(finding => finding.Id == findingId) || 
                !_dbContext.Comments.Any(comment => comment.Id == commentId))
            {
                throw new DbUpdateException("Comment or finding with given id does not exists!");
            }

            _dbContext.FindingComments.Add(new FindingComment 
            { 
                CommentId = commentId,
                FindingId = findingId 
            });

            return await _dbContext.SaveChangesAsync() > 0;
        }

        private CommentReaction GetReaction(int commentId, string userId)
            => _dbContext.CommentReactions
                .FirstOrDefault(reaction => reaction.CommentId == commentId && reaction.UserId == userId);

        public async Task<bool> AddReaction(CommentReaction reaction)
        {
            if(GetReaction(reaction.CommentId, reaction.UserId) != null)
            {
                return false;
            }

            _dbContext.CommentReactions.Add(reaction);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ChangeReaction(CommentReaction newReaction)
        {
            var originalReaction = GetReaction(newReaction.CommentId, newReaction.UserId);

            if (originalReaction is null)
            {
                return false;
            }

            originalReaction.Reaction = newReaction.Reaction;

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteReaction(int commentId, string userId)
        {
            var reaction = GetReaction(commentId, userId);

            if(reaction is null)
            {
                return true;
            }

            _dbContext.CommentReactions.Remove(reaction);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public T GetCommentById<T>(int id, Func<Comment, T> selector)
            => _dbContext.Comments.Include(comment => comment.Creator)
                .Include(comment => comment.Reactions)
                .Where(comment => comment.Id == id)
                .Select(selector).FirstOrDefault();

        public T GetUserReaction<T>(int commentId, string userId, Func<CommentReaction, T> selector)
            => _dbContext.CommentReactions
                .Where(reaction => reaction.CommentId == commentId && reaction.UserId == userId)
                .Select(selector)
                .FirstOrDefault();

        public async Task<bool> AddSubComment(SubComment subComment)
        {
            _dbContext.SubComments.Add(subComment);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public IEnumerable<T> GetSubComments<T>(int mainCommentId, Func<SubComment, T> selector)
            => _dbContext.Comments
                .Include(comment => comment.SubComments)
                .ThenInclude(subcomment => subcomment.Comment)
                .ThenInclude(comment => comment.Creator)
                .Include(comment => comment.SubComments)
                .ThenInclude(subcomment => subcomment.Comment)
                .ThenInclude(comment => comment.Reactions)
                .FirstOrDefault(comment => comment.Id == mainCommentId)?
                .SubComments.Select(selector);

        public async Task<bool> AddPost(Post post)
        {
            _dbContext.Posts.Add(post);

            return await _dbContext.SaveChangesAsync() > 0;
        }

        public IEnumerable<T> GetPosts<T>(Func<Post, T> selector)
            => _dbContext.Posts
                .Include(post => post.Comment)
                .ThenInclude(comment => comment.Creator)
                .Include(post => post.Comment)
                .ThenInclude(comment => comment.Reactions)
                .Select(selector);
    }
}
