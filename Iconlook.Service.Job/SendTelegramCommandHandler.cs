using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Client;
using Iconlook.Message;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Telegram.Bot.Types;

namespace Iconlook.Service.Job
{
    public class SendTelegramCommandHandler : HandlerBase, IHandleMessages<SendTelegramCommand>
    {
        public TelegramClient Telegram { get; set; }
        public IHostEnvironment Environment { get; set; }
        public ServerConfiguration Configuration { get; set; }

        public Task Handle(SendTelegramCommand message, IMessageHandlerContext context)
        {
            var cu = Configuration;
            var chim = Environment;
            return Telegram.SendTextMessageAsync(new ChatId(message.Id), message.Text);
        }
    }
}