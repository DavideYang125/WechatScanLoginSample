namespace Wiwi.Sample.Common.Wp.Events
{
    /// <summary>
    /// 成功的事件应答
    /// </summary>
    public class EventAnswerSuccess : IEventAnswer
    {
        public string FormatString()
        {
            return "success";
        }
    }
}