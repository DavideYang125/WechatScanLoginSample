namespace Wiwi.Sample.Common.Wp.Models.Caches
{
    public class WechatTokenCache
    {
        /// <summary>
        /// access_token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 有效期
        /// </summary>
        public DateTime ExpireTime { get; set; }
    }
}
