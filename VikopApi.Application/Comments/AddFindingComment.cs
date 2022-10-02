﻿namespace VikopApi.Application.Comments
{
    [Service]
    public class AddFindingComment
    {
        private readonly ICommentManager _commentManager;

        public AddFindingComment(ICommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        public async Task<Response> Execute(Request request)
        {
            var comment = new Comment
            {
                Content = request.Content,
                Created = DateTime.Now,
                CreatorId = request.CreatorId,
                Picture = request.Picture
            };

            var res = await _commentManager.AddComment(comment);

            if (!res)
                return null;

            await _commentManager.AddFindingComment(comment.Id, request.FindingId);
            return _commentManager.GetCommentById(comment.Id, comment => new Response
            {
                Content = comment.Content,
                Created = comment.Created.GetTime(),
                CreatorId = comment.CreatorId,
                CreatorName = comment.Creator.UserName,
                Id = comment.Id,
                Reactions = comment.Reactions.SumReactions()
            });
        }

        public class Request
        {
            public string CreatorId { get; set; }
            public string Content { get; set; }
            public int FindingId { get; set; }
            public string? Picture { get; set; }
        }

        public class Response
        {
            public int Id { get; set; }
            public string CreatorId { get; set; }
            public string CreatorName { get; set; }
            public string Content { get; set; }
            public string Created { get; set; }
            public int Reactions { get; set; }
        }
    }
}
