namespace Wiwi.WechatScanLogin.Api.Models.Result
{
    public class QrCodeResult
    {
        /// <summary>
        /// 二维码对应的url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 场景值
        /// </summary>
        public string SceneKey { get; set; }

        public QrCodeResult(string url, string sceneKey)
        {
            Url = url;
            SceneKey = sceneKey;
        }
    }
}
