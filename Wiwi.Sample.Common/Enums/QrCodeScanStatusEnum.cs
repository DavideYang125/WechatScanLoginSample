using System.ComponentModel;

namespace Wiwi.Sample.Common.Enums
{
    /// <summary>
    /// 1:已过期；  2:还未扫码登录   3:已扫码登录
    /// </summary>
    public enum QrCodeScanStatusEnum
    {
        [Description("已过期")] Expired = 1,
        [Description("未扫码登录")] UnLogin = 2,
        [Description("已扫码登录")] Login = 3,
    }
}
