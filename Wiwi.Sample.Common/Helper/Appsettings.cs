using Microsoft.Extensions.Configuration;

namespace Wiwi.Sample.Common.Helper
{

    public class Appsettings
    {
        public static IConfiguration Configuration;

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetConfig<T>(string key)
        {
            return Configuration.GetSection(key).Get<T>();
        }

        public static string GetConfig(string key)
        {
            return Configuration.GetSection(key).Value;
        }
    }
}
