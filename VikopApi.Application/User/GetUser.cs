﻿namespace VikopApi.Application.User
{
    [Service]
    public class GetUser
    {
        private IApplicationUserManager _appUserManager;

        public GetUser(IApplicationUserManager applicationUserManager)
        {
            _appUserManager = applicationUserManager;
        }

        public Response Execute(string id)
            => _appUserManager.GetUserById(id, user => new Response
            {
                Id = user.Id,
                UserName = user.UserName,
                Created = user.Created.ToString("dd.MM.yyyy"),
                Rank = (int)user.Rank
            });

        public class Response
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string Created { get; set; }
            public int Rank { get; set; }
        }
    }
}
