namespace Wiwi.Sample.Common.Wp.Events
{
    /// <summary>
    /// 扫码事件
    /// </summary>
    public class QrCodeScanEvent : EventBase
    {
        public override string MsgType => "event";

        /// <summary>
        /// 事件类型
        /// </summary>
        public static string Event => "scan";

        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值
        /// </summary>
        public string EventKey { get; set; }
    }
}
