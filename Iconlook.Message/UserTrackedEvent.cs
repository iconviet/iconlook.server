using Agiper.Server;

namespace Iconlook.Message
{
    public class UserTrackedEvent : EventBase<UserTrackedEvent>
    {
        public long UserId { get; set; }
        public string Description { get; set; }
    }
}