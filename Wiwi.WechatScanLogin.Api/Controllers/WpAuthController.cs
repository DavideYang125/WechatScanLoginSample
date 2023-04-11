using Microsoft.AspNetCore.Mvc;
using System.Text;
using Wiwi.Sample.Common.Model;
using Wiwi.Sample.Common.Wp.Events;
using Wiwi.WechatScanLogin.Api.Models.Result;
using Wiwi.WechatScanLogin.Api.Models.Vm;
using Wiwi.WechatScanLogin.Api.Services.WpAuth;

namespace Wiwi.WechatScanLogin.Api.Controllers
{
    /// <summary>
    /// 微信登录
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class WpAuthController : ControllerBase
    {
        private readonly ILogger<WpAuthController> _logger;
        private readonly EventContainer _eventContainer;
        private readonly IWpAuthService _wpAuthService;
        public WpAuthController(ILogger<WpAuthController> logger, EventContainer eventContainer, IWpAuthService wpAuthService)
        {
            _logger = logger;
            _eventContainer = eventContainer;
            _wpAuthService = wpAuthService;
            _eventContainer.Subscribed += EventContainer_Subscribed;
            _eventContainer.Unsubscribed += EventContainer_Unsubscribed;
            _eventContainer.QrCodeScaned += EventContainer_ScanQrCode;
        }

        /// <summary>
        /// 验证/时间应答推送地址
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost]
        public async Task<IActionResult> Answer()
        {
            if (HttpMethods.IsGet(HttpContext.Request.Method))
                return HandleToken();
            else
                return await HandleEvent();
        }

        /// <summary>
        /// 获取带参二维码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResult<QrCodeResult>>> CreateQrCode()
        {
            return await _wpAuthService.CreateQrCode();
        }

        /// <summary>
        /// 检测扫码登录状态
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ApiResult<CheckQrCodeLoginResult>> CheckQrCodeLogin([FromBody] CheckQrCodeLoginVm vm)
        {
            return await _wpAuthService.CheckQrCodeLogin(vm);
        }

        /// <summary>
        /// 验证微信Token配置
        /// </summary>
        /// <returns></returns>
        private IActionResult HandleToken()
        {
            var signature = HttpContext.Request.Query["signature"];
            var timestamp = HttpContext.Request.Query["timestamp"];
            var nonce = HttpContext.Request.Query["nonce"];
            var echostr = HttpContext.Request.Query["echostr"];//场景字符串
            if (string.IsNullOrWhiteSpace(signature)
                || string.IsNullOrWhiteSpace(timestamp)
                || string.IsNullOrWhiteSpace(nonce)
                || string.IsNullOrWhiteSpace(echostr))
                return NotFound();

            if (_eventContainer.CheckSignature(signature, timestamp, nonce, "token"))//token可放在配置文件里
            {
                return Content(echostr[0]);
            }
            return NoContent();
        }

        // 微信推送事件处理
        private async Task<IActionResult> HandleEvent()
        {
            var input = HttpContext.Request.Body;
            var postData = string.Empty;
            using (var reader = new StreamReader(input, Encoding.UTF8))
                postData = await reader.ReadToEndAsync();

            var eventAnswer = await _eventContainer.AnswerAsync(postData);
            return Content(eventAnswer.FormatString());
        }

        private async Task<IEventAnswer> EventContainer_Subscribed(SubscribeEvent arg)
        {
            await _wpAuthService.HandleQrCodeScanUser(arg.FromUserName, arg.EventKey, true);

            return new EventAnswerSuccess();
        }

        private async Task<IEventAnswer> EventContainer_Unsubscribed(UnsubscribeEvent arg)
        {
            //TODO：取关事件待定
            return new EventAnswerSuccess();
        }

        private async Task<IEventAnswer> EventContainer_ScanQrCode(QrCodeScanEvent arg)
        {
            await _wpAuthService.HandleQrCodeScanUser(arg.FromUserName, arg.EventKey, false);
            return new EventAnswerSuccess();
        }
    }
}
