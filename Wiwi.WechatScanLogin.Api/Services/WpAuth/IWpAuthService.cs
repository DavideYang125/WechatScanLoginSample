using Wiwi.Sample.Common.Model;
using Wiwi.WechatScanLogin.Api.Models.Result;
using Wiwi.WechatScanLogin.Api.Models.Vm;

namespace Wiwi.WechatScanLogin.Api.Services.WpAuth
{
    public interface IWpAuthService
    {
        Task HandleQrCodeScanUser(string openId, string eventKey, bool newSubscribe = false);
        Task<ApiResult<CheckQrCodeLoginResult>> CheckQrCodeLogin(CheckQrCodeLoginVm vm);
        Task<ApiResult<QrCodeResult>> CreateQrCode();
    }
}
