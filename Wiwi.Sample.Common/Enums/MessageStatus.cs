using System.ComponentModel;

namespace Wiwi.Sample.Common.Enums
{
    /// <summary>
    /// 消息状态
    /// </summary>
    public enum MessageStatus
    {
        /// <summary>
        /// 成功
        /// </summary>
        [Description("成功")]
        Success = 200,

        /// <summary>
        /// 失败 一般应用错误
        /// </summary>
        [Description("失败")]
        Fail = 300,

        /// <summary>
        /// 异常错误
        /// </summary>
        [Description("异常错误")]
        Error = 500
    }
}
