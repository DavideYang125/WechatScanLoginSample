using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Wiwi.Sample.Common.Cache
{
    public interface ICache
    {
        #region object

        Task<bool> SetAsync<T>(string key, T value, int expireSeconds = -1, CSRedis.RedisExistence? exists = null);

        Task<bool> SetAsync<T>(string key, T value, TimeSpan expire, CSRedis.RedisExistence? exists = null);

        Task<T> GetAsync<T>(string key);
        Task<T> GetAsync<T>(string key, Func<Task<T>> operationGetData = null, int expireSeconds = -1) where T : class;

        Task<long> DeleteKeyAsync(params string[] redisKey);

        #endregion object

        #region HASH

        Task<bool> HashSetAsync<T>(string redisKey, string hashField, T value);

        Task<T> HashGetAsync<T>(string redisKey, string hashField);

        Task<bool> HashSetAsync<T>(string key, Dictionary<string, T> keyValues);

        Task<Dictionary<string, T>> HashGetAsync<T>(string redisKey);

        Task<long> HashDeleteAsync(string redisKey, params string[] hashField);

        #endregion HASH

        #region Set

        Task<long> SAddAsync<T>(string key, params T[] members);
        Task<long> SRemAsync<T>(string key, params T[] members);

        Task<bool> SIsMemberAsync(string key, object member);

        #endregion Set

        #region redis分布式锁

        /// <summary>
        /// 加锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expires">过期时间（秒）</param>
        /// <param name="interval">重试时间（毫秒）</param>
        /// <param name="loop">重试次数</param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task<bool> LockAsync(string key, string value, int expires = 3, string message = "", int interval = 100, int loop = 3);

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task UnLockAsync(string key, string value);

        #endregion redis分布式锁
    }
}