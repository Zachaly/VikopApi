﻿using VikopApi.Domain.Enums;

namespace VikopApi.Application.Models
{
    public class UserListItemModel
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public Rank Rank { get; set; } 
    }
}
