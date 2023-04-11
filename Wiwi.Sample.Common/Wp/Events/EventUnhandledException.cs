namespace Wiwi.Sample.Common.Wp.Events
{
    public class EventUnhandledException : Exception
    {
        public EventUnhandledException()
        {
        }
        public EventUnhandledException(string? message) : base(message)
        {
        }
    }
}
