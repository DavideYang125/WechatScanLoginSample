namespace Wiwi.Sample.Common.Wp.Models.Caches
{
    public class QrCodeCache
    {
        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 已扫描/已关注
        /// </summary>
        public bool Scaned { get; set; } = false;

        public string OpenId { get; set; }
    }
}
