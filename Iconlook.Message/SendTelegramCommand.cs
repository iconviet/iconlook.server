using NServiceBus;

namespace Iconlook.Message
{
    public class SendTelegramCommand : ICommand
    {
        public long Id { get; set; }
        public string Text { get; set; }
    }
}