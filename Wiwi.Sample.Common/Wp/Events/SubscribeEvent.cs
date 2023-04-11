namespace Wiwi.Sample.Common.Wp.Events
{
    /// <summary>
    /// 订阅事件
    /// </summary>
    public class SubscribeEvent : EventBase
    {
        public override string MsgType => "event";

        /// <summary>
        /// 事件类型
        /// </summary>
        public static string Event => "subscribe";

        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
    }
}