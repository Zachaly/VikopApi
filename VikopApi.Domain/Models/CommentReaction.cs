﻿using VikopApi.Domain.Enums;

namespace VikopApi.Domain.Models
{
    public class CommentReaction
    {

        public int CommentId { get; set; }
        public Comment Comment { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Reaction Reaction { get; set; }
    }
}