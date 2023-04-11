using CSRedis;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Wiwi.Sample.Common.Cache
{
    public class RedisCache : ICache
    {
        private readonly ILogger<RedisCache> _logger;

        public RedisCache(ILogger<RedisCache> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///  设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <param name="expireSeconds">过期(秒单位)</param>
        /// <param name="exists"> Nx, Xx</param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(string key, T value, int expireSeconds = -1, RedisExistence? exists = null)
        {
            return await RedisHelper.SetAsync(key, value, expireSeconds, exists);
        }

        /// <summary>
        ///  设置指定 key 的值，所有写入参数object都支持string | byte[] | 数值 | 对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <param name="expire">过期时间(TimeSpan)</param>
        /// <param name="exists"> Nx, Xx</param>
        /// <returns></returns>
        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan expire, RedisExistence? exists = null)
        {
            return await RedisHelper.SetAsync(key, value, expire, exists);
        }

        /// <summary>
        /// 获取指定 key 的值
        /// </summary>
        /// <typeparam name="T"> byte[] 或其他类型</typeparam>
        /// <param name="key">不含prefix前辍param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            return await RedisHelper.GetAsync<T>(key);
        }

        /// <summary>
        /// 获取指定 key 的值 ，如果为空执行委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="expireSeconds"></param>
        /// <param name="operationGetData"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key, Func<Task<T>> operationGetData = null, int expireSeconds = -1) where T : class
        {
            //获取缓存
            var data = await RedisHelper.GetAsync<T>(key);
            if (data == null && operationGetData != null)
            {
                //缓存不存在，执行委托
                data = await operationGetData();
                if (data != null)
                {
                    await RedisHelper.SetAsync(key, data, expireSeconds);
                }
            }
            return data;
        }

        /// <summary>
        /// 用于在 key 存在时删除 key
        /// </summary>
        /// <param name="redisKey">不含prefix前辍</param>
        /// <returns></returns>
        public async Task<long> DeleteKeyAsync(params string[] redisKey)
        {
            return await RedisHelper.DelAsync(redisKey);
        }

        /// <summary>
        /// 将哈希表 key 中的字段 field 的值设为 value
        /// </summary>
        /// <param name="redisKey">不含prefix前辍</param>
        /// <param name="hashField">字段</param>
        /// <param name="value">值</param>
        /// <typeparam name="T"></typeparam>
        /// <returns>如果字段是哈希表中的一个新建字段，并且值设置成功，返回true。如果哈希表中域字段已经存在且旧值已被新值覆盖，返回false</returns>
        public async Task<bool> HashSetAsync<T>(string redisKey, string hashField, T value)
        {
            return await RedisHelper.HSetAsync(redisKey, hashField, value);
        }

        /// <summary>
        /// 获取存储在哈希表中指定字段的值
        /// </summary>
        /// <param name="redisKey">不含prefix前辍</param>
        /// <param name="hashField">字段</param>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <returns></returns>
        public async Task<T> HashGetAsync<T>(string redisKey, string hashField)
        {
            return await RedisHelper.HGetAsync<T>(redisKey, hashField);
        }

        /// <summary>
        /// 同时将多个 field-value (域-值)对设置到哈希表 key 中
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="keyValues"> key1 value1 [key2 value2]</param>
        /// <returns></returns>
        public async Task<bool> HashSetAsync<T>(string key, Dictionary<string, T> keyValues)
        {
            List<object> objectarr = new List<object>();
            foreach (var value in keyValues)
            {
                objectarr.Add(value.Key);
                objectarr.Add(value.Value);
            }
            return await RedisHelper.HMSetAsync(key, objectarr.ToArray());
        }

        /// <summary>
        /// 获取在哈希表中指定 key 的所有字段和值
        /// </summary>
        /// <param name="redisKey">不含prefix前辍</param>
        /// <typeparam name="T">byte[] 或其他类型</typeparam>
        /// <returns></returns>
        public async Task<Dictionary<string, T>> HashGetAsync<T>(string redisKey)
        {
            return await RedisHelper.HGetAllAsync<T>(redisKey);
        }

        /// <summary>
        /// 删除一个或多个哈希表字段
        /// </summary>
        /// <param name="redisKey">不含prefix前辍</param>
        /// <param name="hashField">字段</param>
        /// <returns></returns>
        public async Task<long> HashDeleteAsync(string redisKey, params string[] hashField)
        {
            return await RedisHelper.HDelAsync(redisKey, hashField);
        }

        /// <summary>
        ///  向集合添加一个或多个成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="members">一个或多个成员</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<long> SAddAsync<T>(string key, params T[] members)
        {
            return await RedisHelper.SAddAsync(key, members);
        }
        /// <summary>
        ///  移除一个或多个成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="members">一个或多个成员</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<long> SRemAsync<T>(string key, params T[] members)
        {
            return await RedisHelper.SRemAsync(key, members);
        }

        /// <summary>
        /// 将给定 key 的值设为 value ，并返回 key 的旧值(old value)
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="value">值</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<T> GetSetAsync<T>(string key, object value)
        {
            return await RedisHelper.GetSetAsync<T>(key, value);
        }

        
        /// <summary>
        /// 判断 member 元素是否是集合 key 的成员
        /// </summary>
        /// <param name="key">不含prefix前辍</param>
        /// <param name="member">成员</param>
        /// <returns></returns>
        public async Task<bool> SIsMemberAsync(string key, object member)
        {
            return await RedisHelper.SIsMemberAsync(key, member);
        }

        #region redis锁
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
        public async Task<bool> LockAsync(string key, string value, int expires = 3, string message = "", int interval = 100, int loop = 3)
        {
            var lockSuccess = false;
            for (int i = 0; i < loop; i++)
            {
                lockSuccess = await RedisHelper.SetAsync(key, value, expires, RedisExistence.Nx);
                if (lockSuccess)
                    break;
                //判断是否是当前节点的锁
                var oldValue = await RedisHelper.GetAsync(key);
                if (oldValue != value)
                    break;

                //重锁
                lockSuccess = await RedisHelper.SetAsync(key, value, expires);
                if (lockSuccess)
                    break;

                if (interval > 0 && loop > 1)
                    Thread.Sleep(interval);
            }
            //记录加锁信息
            if (!string.IsNullOrWhiteSpace(message))
                _logger.LogInformation($"机器名：{Environment.MachineName}，KV:[{key}:{value}]，加锁消息：{message}，结果：{lockSuccess}");

            return lockSuccess;
        }

        /// <summary>
        /// 解锁
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task UnLockAsync(string key, string value)
        {
            //判断是否是当前节点的锁
            var oldValue = await RedisHelper.GetAsync(key);
            if (oldValue != value)
                return;
            //移除锁
            await RedisHelper.DelAsync(key);
        }

        #endregion redis锁
    }
}