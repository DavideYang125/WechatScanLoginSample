using Newtonsoft.Json;

namespace Wiwi.Sample.Common.Wp.Models.Response
{
    public class GetUserInfoResponse : BaseWeChatResponse
    {
        /// <summary>
        /// 关注公众号
        /// </summary>
        [JsonProperty("subscribe")]
        public int Subscribed { get; set; }

        /// <summary>
        /// 用户的标识
        /// </summary>
        [JsonProperty("openid")]
        public string OpenId { get; set; }

        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。
        /// </summary>
        [JsonProperty("unionid")]
        public string UnionId { get; set; }

        /// <summary>
        /// 二维码扫码场景描述
        /// </summary>
        [JsonProperty("qr_scene_str")]
        public string QrSceneStr { get; set; }
    }
}
