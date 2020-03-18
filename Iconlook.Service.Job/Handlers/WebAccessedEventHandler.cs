using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Iconlook.Client;
using Iconlook.Message;
using NServiceBus;
using Telegram.Bot.Types;

namespace Iconlook.Service.Job.Handlers
{
    public class WebAccessedEventHandler : HandlerBase, IHandleMessages<WebAccessedEvent>
    {
        public TelegramClient Telegram { get; set; }
        public HostConfiguration Configuration { get; set; }

        public Task Handle(WebAccessedEvent message, IMessageHandlerContext context)
        {
            return Configuration.Environment == Environment.Localhost
                ? Task.CompletedTask
                : Telegram.SendTextMessageAsync(new ChatId(-1001449380420), message.Description);
        }
    }
}