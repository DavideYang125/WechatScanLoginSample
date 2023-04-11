using Wiwi.Sample.Common.Enums;

namespace Wiwi.WechatScanLogin.Api.Models.Result
{
    public class CheckQrCodeLoginResult
    {
        /// <summary>
        /// 扫码状态
        /// 1:已过期；  2:还未扫码登录；   3:已扫码登录
        /// </summary>
        public QrCodeScanStatusEnum QrCodeScanStatus { get; set; }
    }
}
