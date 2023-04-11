using Wiwi.Sample.Common.Enums;
using Wiwi.Sample.Common.Helper;

namespace Wiwi.Sample.Common.Exceptions
{

    /// <summary>
    /// 自定义异常类
    /// </summary>
    public class CustomException : Exception
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public MessageStatus Code { get; set; }

        public new string Message { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomException()
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message"></param>
        public CustomException(string message) : base(message)
        {
            Message = message;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public CustomException(MessageStatus code, string message) : base(message)
        {
            Code = code;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="code"></param>
        public CustomException(MessageStatus code)
        {
            Code = code;
            Message = EnumHelper.GetEnumDesc<MessageStatus>(code.GetHashCode());
        }
    }
}
