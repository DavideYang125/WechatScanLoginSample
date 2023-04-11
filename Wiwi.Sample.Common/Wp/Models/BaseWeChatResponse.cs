using Newtonsoft.Json;

namespace Wiwi.Sample.Common.Wp.Models
{
    public class BaseWeChatResponse
    {
        /// <summary>
        /// 错误码
        /// </summary>
        [JsonProperty("errcode")]
        public int ErrorCode { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        [JsonProperty("errmsg")]
        public string ErrorMessage { get; set; }
    }
}
