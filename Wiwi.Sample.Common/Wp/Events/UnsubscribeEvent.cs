namespace Wiwi.Sample.Common.Wp.Events
{
    /// <summary>
    /// 取消订阅事件
    /// </summary>
    public class UnsubscribeEvent : EventBase
    {
        public override string MsgType => "event";

        /// <summary>
        /// 事件类型
        /// </summary>
        public static string Event => "unsubscribe";
    }
}