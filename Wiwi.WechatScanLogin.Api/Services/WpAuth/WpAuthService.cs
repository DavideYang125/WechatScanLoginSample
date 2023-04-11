using Wiwi.Sample.Common.Cache;
using Wiwi.Sample.Common.Enums;
using Wiwi.Sample.Common.Extensions;
using Wiwi.Sample.Common.Model;
using Wiwi.Sample.Common.Model.Caches;
using Wiwi.Sample.Common.Wp.Models.Caches;
using Wiwi.Sample.Common.Wp.Models.Requests;
using Wiwi.Sample.Common.Wp.Models.Response;
using Wiwi.WechatScanLogin.Api.Models.Result;
using Wiwi.WechatScanLogin.Api.Models.Vm;

namespace Wiwi.WechatScanLogin.Api.Services.WpAuth
{
    public class WpAuthService : IWpAuthService
    {
        private readonly ILogger<WpAuthService> _logger;
        private readonly HttpClient _client;
        private const string _qrcodeUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
        private readonly ICache _cache;
        public WpAuthService(ILogger<WpAuthService> logger, IHttpClientFactory httpClientFactory, ICache cache)
        {
            _logger = logger;
            _client = httpClientFactory.CreateClient();
            _cache = cache;
        }

        /// <summary>
        /// 获取带参数的二维码
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResult<QrCodeResult>> CreateQrCode()
        {
            //微信公众号的access_token缓存需要另写定时任务维护，定时更新保存在reids里。这里只从缓存里取数据
            var tokenCache = await _cache.GetAsync<WechatTokenCache>(GlobalCacheKey.WechatAccessTokenKey);
            var token = string.Empty;
            if (tokenCache != null) token = tokenCache.AccessToken;
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogError($"获取access_token失败");
                return ApiResult<QrCodeResult>.Fail("获取登录二维码失败");
            }
            var url = string.Format(_qrcodeUrl, token);

            var sceneStr = Guid.NewGuid().ToString();

            QrCodeRequest request = new QrCodeRequest() { action_info = new QrCodeAction(new QrCodeScene(sceneStr)) };
            var result = await _client.PostAsync<QrCodeRequest, QrCodeResponse>(url, request);
            if (result != null && result.ErrorCode == 0)
            {
                var key = string.Format(GlobalCacheKey.QrScanLoginKey, sceneStr);
                var qrCodeCache = new QrCodeCache() { ExpireTime = DateTime.Now.AddSeconds(result.expire_seconds), OpenId = string.Empty, Scaned = false };

                await _cache.SetAsync(key, qrCodeCache, 60 * 5);

                return ApiResult<QrCodeResult>.Success(new QrCodeResult(result.url, sceneStr));
            }

            _logger.LogError($"获取access_token失败,ErrorCode:{result?.ErrorCode};ErrorMessage:{result?.ErrorMessage}");
            return ApiResult<QrCodeResult>.Fail("获取登录二维码失败");
        }

        public async Task<ApiResult<CheckQrCodeLoginResult>> CheckQrCodeLogin(CheckQrCodeLoginVm vm)
        {
            if (vm is null || string.IsNullOrEmpty(vm?.SceneKey))
            {
                return ApiResult<CheckQrCodeLoginResult>.Fail("参数不能为空");
            }

            var key = string.Format(GlobalCacheKey.QrScanLoginKey, vm.SceneKey);
            var qrCodeCache = await RedisHelper.GetAsync<QrCodeCache>(key);
            var result = new CheckQrCodeLoginResult();
            if (qrCodeCache is null)
            {
                result.QrCodeScanStatus = QrCodeScanStatusEnum.Expired;
                return ApiResult<CheckQrCodeLoginResult>.Success(result);
            }
            if (qrCodeCache.ExpireTime <= DateTime.Now)
            {
                result.QrCodeScanStatus = QrCodeScanStatusEnum.Expired;
            }
            else if (!qrCodeCache.Scaned)
            {
                result.QrCodeScanStatus = QrCodeScanStatusEnum.UnLogin;
            }
            if (qrCodeCache.Scaned)
            {
                result.QrCodeScanStatus = QrCodeScanStatusEnum.Login;
            }

            //也可根据实际情况，返回JWT token等数据

            return ApiResult<CheckQrCodeLoginResult>.Success(result);
        }

        /// <summary>
        /// 处理扫码事件（新用户是关注事件，否则是扫码事件）
        /// </summary>
        /// <param name="openId"></param>
        /// <param name="newSubscribe"></param>
        /// <returns></returns>
        public async Task HandleQrCodeScanUser(string openId, string eventKey, bool newSubscribe = false)
        {
            var qrSceneStr = eventKey.Replace("qrscene_", "").Trim();

            var key = string.Format(GlobalCacheKey.QrScanLoginKey, qrSceneStr);
            var qrCodeCache = await _cache.GetAsync<QrCodeCache>(key);
            if (qrCodeCache != null)
            {
                qrCodeCache.Scaned = true;
                qrCodeCache.OpenId = openId;

                //TODO:可根据实际情况做数据库相关操作

                await _cache.SetAsync(key, qrCodeCache, 60 * 5);
            }
        }
    }
}
