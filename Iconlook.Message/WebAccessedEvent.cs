using Agiper.Server;

namespace Iconlook.Message
{
    public class WebAccessedEvent : EventBase<WebAccessedEvent>
    {
        public string Description { get; set; }
    }
}