using System.Xml;
using Wiwi.Sample.Common.Exceptions;
using Wiwi.Sample.Common.Extensions;

namespace Wiwi.Sample.Common.Wp.Events
{
    public class EventContainer
    {
        /// <summary>
        /// 订阅事件
        /// </summary>
        public event Func<SubscribeEvent, Task<IEventAnswer>> Subscribed;

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        public event Func<UnsubscribeEvent, Task<IEventAnswer>> Unsubscribed;

        /// <summary>
        /// 扫码事件
        /// </summary>
        public event Func<QrCodeScanEvent, Task<IEventAnswer>> QrCodeScaned;

        /// <summary>
        /// 事件应答
        /// </summary>
        /// <param name="eventData">事件的 xml 格式化文本</param>
        /// <returns></returns>
        public async Task<IEventAnswer> AnswerAsync(string eventData)
        {
            var xmlDoc = new XmlDocument();

            try
            {
                xmlDoc.LoadXml(eventData);
                Console.WriteLine(eventData);
            }
            catch (Exception)
            {
                return new EventAnswerEmpty();
            }

            var msgTypeNode = xmlDoc.SelectSingleNode("/xml/MsgType");

            if (msgTypeNode == null)
            {
                return new EventAnswerEmpty();
            }

            switch (msgTypeNode.InnerText)
            {
                case "event":
                    return await HandleEventAsync(xmlDoc);
                default:
                    return new EventAnswerSuccess();
            }
        }

        // 处理事件
        private async Task<IEventAnswer> HandleEventAsync(XmlDocument xmlDoc)
        {
            var eventName = xmlDoc.SelectSingleNode("/xml/Event");

            if (eventName.InnerText == SubscribeEvent.Event)
            {
                return await OnSubscribedAsync(xmlDoc);
            }
            else if (eventName.InnerText == UnsubscribeEvent.Event)
            {
                return await OnUnsubscribedAsync(xmlDoc);
            }
            else if (eventName.InnerText.ToLower() == QrCodeScanEvent.Event)
            {
                return await OnQrCodeScanAsync(xmlDoc);
            }
            else
            {
                return new EventAnswerSuccess();
            }
        }


        // 订阅事件
        private async Task<IEventAnswer> OnSubscribedAsync(XmlDocument xmlDoc)
        {
            if (Subscribed == null)
            {
                throw new CustomException("未绑定订阅事件。");
            }

            var e = new SubscribeEvent
            {
                CreateTime = long.Parse(xmlDoc.SelectSingleNode("/xml/CreateTime").InnerText),
                FromUserName = xmlDoc.SelectSingleNode("/xml/FromUserName").InnerText,
                ToUserName = xmlDoc.SelectSingleNode("/xml/ToUserName").InnerText,
                EventKey = xmlDoc.SelectSingleNode("/xml/EventKey")?.InnerText,
                Ticket = xmlDoc.SelectSingleNode("/xml/Ticket")?.InnerText
            };

            var answer = await Subscribed.Invoke(e);

            return answer;
        }

        // 解除订阅事件
        private async Task<IEventAnswer> OnUnsubscribedAsync(XmlDocument xmlDoc)
        {
            if (Unsubscribed == null)
            {
                throw new EventUnhandledException("未绑定取消订阅事件处理过程。");
            }

            var e = new UnsubscribeEvent
            {
                CreateTime = long.Parse(xmlDoc.SelectSingleNode("/xml/CreateTime").InnerText),
                FromUserName = xmlDoc.SelectSingleNode("/xml/FromUserName").InnerText,
                ToUserName = xmlDoc.SelectSingleNode("/xml/ToUserName").InnerText
            };

            var answer = await Unsubscribed.Invoke(e);

            return answer;
        }

        // 扫码事件
        private async Task<IEventAnswer> OnQrCodeScanAsync(XmlDocument xmlDoc)
        {
            if (Unsubscribed == null)
            {
                throw new EventUnhandledException("未绑定扫码事件。");
            }

            var e = new QrCodeScanEvent
            {
                CreateTime = long.Parse(xmlDoc.SelectSingleNode("/xml/CreateTime").InnerText),
                FromUserName = xmlDoc.SelectSingleNode("/xml/FromUserName").InnerText,
                ToUserName = xmlDoc.SelectSingleNode("/xml/ToUserName").InnerText,
                EventKey = xmlDoc.SelectSingleNode("/xml/EventKey")?.InnerText,
            };

            var answer = await QrCodeScaned.Invoke(e);

            return answer;
        }


        /// <summary>
        /// 验证微信Token配置
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public bool CheckSignature(string signature, string timestamp, string nonce, string token)
        {
            if (string.IsNullOrWhiteSpace(signature))
                return false;
            var arr = new[] { timestamp, nonce, token };
            Array.Sort(arr);
            var localSign = string.Concat(arr).GetSHA1Hash();
            return signature.Equals(localSign, StringComparison.OrdinalIgnoreCase);
        }
    }
}
