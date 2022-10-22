using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Application;
using VikopApi.Application.Comments;
using VikopApi.Database;
using VikopApi.Domain.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection @this)
        {
            var types = new List<TypeInfo>();
            types.AddRange(typeof(AppDbContext).Assembly.DefinedTypes);
            types.AddRange(typeof(CommentFactory).Assembly.DefinedTypes);
            types.AddRange(typeof(AuthManager).Assembly.DefinedTypes);

            var services = types.
                Where(type => type.GetTypeInfo().GetCustomAttribute<Implementation>() != null);

            @this.AddHttpContextAccessor();

            foreach (var service in services)
            {
                var attribute = service.GetCustomAttribute<Implementation>();
                @this.AddScoped(attribute.Interface, service);
            }
            return @this;
        }
    }
}