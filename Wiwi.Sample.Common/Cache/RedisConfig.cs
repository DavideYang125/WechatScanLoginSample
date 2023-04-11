using System;
using System.Collections.Generic;
using System.Text;

namespace Wiwi.Sample.Common.Cache
{
    public class RedisConfig
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 实例名
        /// </summary>
        public string InstanceName { get; set; }
    }
}
