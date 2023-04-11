using CSRedis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Wiwi.Sample.Common.Cache
{
    public static class RedisCacheExtensions
    {
        public static void AddRedisCacheService(this IServiceCollection services, Func<RedisConfig> func)
        {
            var config = func.Invoke();

            services.AddRedisCacheService(config);
        }

        public static void AddRedisCacheService(this IServiceCollection services, RedisConfig config)
        {
            if (config == null)
                throw new ArgumentNullException("redis config is null");

            RedisHelper.Initialization(new CSRedisClient($"{config.ConnectionString},prefix={config.InstanceName},testcluster=false"));

            services.AddTransient<ICache, RedisCache>();
        }
    }
}