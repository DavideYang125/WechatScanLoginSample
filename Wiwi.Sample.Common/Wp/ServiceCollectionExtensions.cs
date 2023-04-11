using Microsoft.Extensions.DependencyInjection;
using Wiwi.Sample.Common.Wp.Events;

namespace Wiwi.Sample.Common.Wp
{
    public static class ServiceCollectionExtensions
    {
        public static void AddDefaultWeChat(this IServiceCollection services)
        {
            services.AddTransient<EventContainer>();
        }
    }
}
