using System.Reflection;
using VikopApi.Application;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceRegister
    {
        /// <summary>
        /// Registers classed marked as 'Service' in VikopApi.Application
        /// </summary>
        public static IServiceCollection AddApplicationServices(this IServiceCollection @this)
        {
            var serviceType = typeof(Service);

            var definedTypes = serviceType.Assembly.DefinedTypes;

            var services = definedTypes.
                Where(type => type.GetTypeInfo().GetCustomAttribute<Service>() != null);

            foreach (var service in services)
            {
                @this.AddTransient(service);
            }

            return @this;
        }
    }
}