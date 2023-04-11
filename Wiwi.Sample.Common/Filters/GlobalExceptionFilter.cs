using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Wiwi.Sample.Common.Enums;
using Wiwi.Sample.Common.Exceptions;
using Wiwi.Sample.Common.Model;

namespace Wiwi.Sample.Common.Filters
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class GlobalExceptionFilter : Attribute, IAsyncExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            //判断异常是否已经处理
            if (!context.ExceptionHandled)
            {
                var myException = GetMyException(context.Exception);
                await Log(context);
                if (myException != null)
                {
                    var code = myException.Code.GetHashCode();
                    if (code.Equals(0))
                    {
                        code = MessageStatus.Error.GetHashCode();
                    }
                    context.Result = new JsonResult(ApiResult.Message((MessageStatus)code, myException.Message));
                }
                else
                    context.Result = new JsonResult(ApiResult.Message(MessageStatus.Error,"服务异常"));

                context.ExceptionHandled = true;
            }
        }

        /// <summary>
        /// 获取自定义异常
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private CustomException GetMyException(Exception exception)
        {
            if (exception == null)
                return null;
            if (exception is CustomException)
                return exception as CustomException;
            return GetMyException(exception.InnerException);
        }

        /// <summary>
        /// 记录日志信息
        /// </summary>
        /// <param name="context"></param>
        private async Task Log(ExceptionContext context)
        {
            if (context.Exception == null) return;
            context.HttpContext.Request.EnableBuffering();
            var requestInfo = await ReadRequest(context.HttpContext);
            _logger.LogInformation("请求日志：" + requestInfo);
            string message = JsonConvert.SerializeObject(context.Exception);
            _logger.LogError("GlobalExceptionFilter:" + message);
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task<string> ReadRequest(Microsoft.AspNetCore.Http.HttpContext context)
        {
            var request = context.Request;
            var url = request.GetEncodedUrl();
            var message = $"[请求地址]:{url}";
            if (!(request.ContentLength > 0)) return message;
            request.Body.Position = 0;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer).Replace("\r\n", "\n").Replace("\n", "");

            message += $@";[请求参数]:{bodyAsText}";
            return message;
        }
    }
}
