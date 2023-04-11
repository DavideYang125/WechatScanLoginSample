namespace Wiwi.Sample.Common.Wp.Models.Response
{
    public class QrCodeResponse : BaseWeChatResponse
    {
        /// <summary>
        /// 二维码ticket，凭借此ticket可以在有效时间内换取二维码
        /// </summary>
        public string ticket { get; set; }

        /// <summary>
        /// 该二维码有效时间，以秒为单位
        /// </summary>
        public long expire_seconds { get; set; }

        /// <summary>
        /// 二维码图片解析后的地址
        /// </summary>
        public string url { get; set; }
    }
}
