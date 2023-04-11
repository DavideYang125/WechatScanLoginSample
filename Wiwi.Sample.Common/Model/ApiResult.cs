using Microsoft.AspNetCore.Mvc;
using Wiwi.Sample.Common.Enums;

namespace Wiwi.Sample.Common.Model
{
    /// <summary>
    /// 通用返回信息类
    /// </summary>
    public class ApiResult<T> : ApiResult
    {
        /// <summary>
        /// 返回数据集合
        /// </summary>
        public T response { get; set; }


        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <returns></returns>
        public static ApiResult<T> Success(T response)
        {
            return Message(MessageStatus.Success, "success", response);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static ApiResult<T> Fail(string msg)
        {
            return Message(MessageStatus.Fail, msg, default);
        }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static ApiResult<T> Message(MessageStatus status, string msg)
        {
            return Message(status, msg, default);
        }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="status">失败/成功</param>
        /// <param name="msg">消息</param>
        /// <param name="response">数据</param>
        /// <returns></returns>
        public static ApiResult<T> Message(MessageStatus status, string msg, T response)
        {
            return new ApiResult<T>() { status = status, msg = msg, response = response };
        }
    }

    public class ApiResult
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public MessageStatus status { get; set; } = MessageStatus.Success;
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool success { get { return this.status == MessageStatus.Success; } }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string msg { get; set; } = "success";

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static ApiResult Success(string msg = "success")
        {
            return Message(MessageStatus.Success, msg);
        }
        /// <summary>
        /// 返回失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static ApiResult Fail(string msg)
        {
            return Message(MessageStatus.Fail, msg);
        }



        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static ApiResult Message(MessageStatus status, string msg)
        {
            return new ApiResult() { status = status, msg = msg };
        }

        /// <summary>
        /// 返回消息
        /// </summary>
        /// <param name="status">状态</param>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static ApiResult Message(MessageStatus status)
        {
            return new ApiResult() { status = status };
        }


        #region  控制器返回

        /// <summary>
        /// 控制器返回失败
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        public static OkObjectResult ErrorResult(string msg)
        {
            return new OkObjectResult(Fail(msg));
        }

        /// <summary>
        /// 控制器返回成功
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        public static OkObjectResult Ok<T>(T data)
        {
            return new OkObjectResult(ApiResult<T>.Success(data));
        }

        /// <summary>
        /// 控制器返回成功
        /// </summary>
        /// <returns></returns>
        public static OkObjectResult Ok()
        {
            return new OkObjectResult(ApiResult.Success());
        }

        #endregion
    }
}
