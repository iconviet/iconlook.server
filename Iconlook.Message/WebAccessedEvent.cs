using Agiper.Server;

namespace Iconlook.Message
{
    public class WebAccessedEvent : EventBase<WebAccessedEvent>
    {
        public string Url { get; set; }
        public string Referer { get; set; }
        public string Address { get; set; }
        public string UserAgent { get; set; }
        public string UserHashId { get; set; }
        public string IconString { get; set; }
        public string BodyString { get; set; }
    }
}