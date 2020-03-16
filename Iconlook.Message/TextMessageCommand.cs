using NServiceBus;

namespace Iconlook.Message
{
    public class TextMessageCommand : ICommand
    {
        public long Id { get; set; }
        public string Text { get; set; }
    }
}