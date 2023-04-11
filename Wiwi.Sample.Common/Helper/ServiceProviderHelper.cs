using Microsoft.Extensions.DependencyInjection;

namespace Wiwi.Sample.Common.Helper
{
    public static class ServiceProviderHelper
    {
        public static IServiceProvider provider;

        public static IServiceProvider SetServiceProvider(this IServiceProvider provider)
        {
            ServiceProviderHelper.provider = provider;
            return provider;
        }
        /// <summary>
        /// 获取服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            return provider.GetService<T>();
        }
    }
}
