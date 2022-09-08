﻿using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Database;
using VikopApi.Domain.Infractructure;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InfrastructureRegister
    {
        /// <summary>
        /// Add managers used in this application
        /// </summary>
        public static IServiceCollection AddApplicationInfrastucture(this IServiceCollection @this)
        {
            @this.AddHttpContextAccessor();
            @this.AddScoped<IApplicationUserManager, ApplicationUserManager>();
            @this.AddScoped<IAuthManager, AuthManager>();
            return @this;
        }
    }
}