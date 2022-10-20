using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Api.Infrastructure.FileManager;
using VikopApi.Application.Factories;
using VikopApi.Application.Factories.Abstractions;
using VikopApi.Application.Tags;
using VikopApi.Application.Tags.Abtractions;
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
            @this.AddScoped<IFindingManager, FindingManager>();
            @this.AddScoped<ICommentManager, CommentManager>();
            @this.AddScoped<IAuthManager, AuthManager>();
            @this.AddScoped<IFileManager, FileManager>();
            @this.AddScoped<IPostManager, PostManager>();
            @this.AddScoped<ITagManager, TagManager>();
            return @this;
        }

        public static IServiceCollection AddFactiories(this IServiceCollection @this)
        {
            @this.AddScoped<IPostFactory, PostFactory>();
            @this.AddScoped<ITagFactory, TagFactory>();
            @this.AddScoped<ICommentFactory, CommentFactory>();

            return @this;
        }

        public static IServiceCollection AddServices(this IServiceCollection @this)
        {
            @this.AddScoped<ITagService, TagService>();

            return @this;
        }
    }
}