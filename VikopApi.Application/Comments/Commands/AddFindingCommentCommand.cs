﻿using MediatR;
using Microsoft.AspNetCore.Http;
using VikopApi.Application.Auth.Abstractions;
using VikopApi.Application.Command.Abstractions;
using VikopApi.Application.Comments.Abstractions;
using VikopApi.Application.Files.Abstractions;
using VikopApi.Application.Models;
using VikopApi.Application.Models.Requests;

namespace VikopApi.Application.Comments.Commands
{
    public class AddFindingCommentCommand : IRequest<DataCommandResponseModel<CommentModel>>
    {
        public int FindingId { get; set; }
        public string Content { get; set; }
        public IFormFile? Picture { get; set; }
    }

    public class AddFindingCommentHandler : IRequestHandler<AddFindingCommentCommand, DataCommandResponseModel<CommentModel>>
    {
        private readonly IFileService _fileService;
        private readonly IAuthService _authService;
        private readonly ICommentService _commentService;
        private readonly ICommandResponseFactory _commandResponseFactory;

        public AddFindingCommentHandler(IAuthService authService, ICommentService commentService,
            IFileService fileService, ICommandResponseFactory commandResponseFactory)
        {
            _fileService = fileService;
            _authService = authService;
            _commentService = commentService;
            _commandResponseFactory = commandResponseFactory;
        }

        public async Task<DataCommandResponseModel<CommentModel>> Handle(AddFindingCommentCommand request, CancellationToken cancellationToken)
        {
            var commentRequest = new AddFindingCommentRequest
            {
                Content = request.Content,
                CreatorId = _authService.GetCurrentUserId(),
                FindingId = request.FindingId,
                Picture = ""
            };

            if (request.Picture != null)
            {
                commentRequest.Picture = await _fileService.SaveCommentPicture(request.Picture);
            }

            var res = await _commentService.AddFindingComment(commentRequest);

            return _commandResponseFactory.CreateSuccess(res);
        }
    }
}
