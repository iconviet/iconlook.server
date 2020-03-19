using NServiceBus;

namespace Iconlook.Message
{
    public class TextMessageCommand : ICommand
    {
        public long ChatId { get; set; }
        public string Text { get; set; }
    }
}