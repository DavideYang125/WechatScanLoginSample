namespace Wiwi.Sample.Common.Wp.Events
{
    /// <summary>
    /// 基础信息结构
    /// </summary>
    public abstract class EventBase
    {
        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 发送方帐号（一个 OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public abstract string MsgType { get; }

        /// <summary>
        /// 将消息创建时间变为本地时间
        /// </summary>
        /// <returns></returns>
        public DateTime CreateTimeToLocal()
        {
            var timeOffset = DateTimeOffset.FromUnixTimeSeconds(CreateTime);

            return timeOffset.LocalDateTime;
        }
    }
}