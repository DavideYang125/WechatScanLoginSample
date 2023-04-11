using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wiwi.Sample.Common.Model.Caches
{
    public class GlobalCacheKey
    {
        /// <summary>
        /// 微信accesstoken
        /// </summary>
        public const string WechatAccessTokenKey = "message:accesstoken";

        /// <summary>
        /// 扫码登录
        /// </summary>
        public const string QrScanLoginKey = "uc:qrscan:login:scene:{0}";
    }
}
