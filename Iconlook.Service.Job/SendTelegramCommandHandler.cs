using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Iconlook.Client;
using Iconlook.Message;
using NServiceBus;
using Telegram.Bot.Types;

namespace Iconlook.Service.Job
{
    public class SendTelegramCommandHandler : HandlerBase, IHandleMessages<SendTelegramCommand>
    {
        public TelegramClient Telegram { get; set; }
        public HostConfiguration Configuration { get; set; }

        public Task Handle(SendTelegramCommand message, IMessageHandlerContext context)
        {
            return Configuration.Environment == Environment.Localhost
                ? Task.CompletedTask
                : Telegram.SendTextMessageAsync(new ChatId(message.Id), message.Text);
        }
    }
}