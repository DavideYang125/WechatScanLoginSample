namespace Wiwi.Sample.Common.Wp.Models.Requests
{
    public class QrCodeRequest
    {
        /// <summary>
        /// 二维码过期时间（单位:秒）
        /// </summary>
        public long expire_seconds { get; set; } = 60 * 5;

        public string action_name { get; set; } = "QR_STR_SCENE";

        public QrCodeAction action_info { get; set; }
    }

    public class QrCodeAction
    {
        public QrCodeAction(QrCodeScene scene)
        {
            this.scene = scene;
        }

        public QrCodeScene scene { get; set; }
    }

    public class QrCodeScene
    {
        public QrCodeScene(string scene_str)
        {
            this.scene_str = scene_str;
        }

        public string scene_str { get; set; }
    }
}
