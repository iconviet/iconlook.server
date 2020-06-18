using System.Threading.Tasks;
using Iconlook.Common;
using Iconlook.Message;
using Iconviet;
using Iconviet.Server;
using NServiceBus;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Iconlook.Service.Job.Handlers
{
    public class TextMessageCommandHandler : HandlerBase, IHandleMessages<TextMessageCommand>
    {
        public TelegramApiClient Telegram { get; set; }
        public HostConfiguration Configuration { get; set; }

        public Task Handle(TextMessageCommand message, IMessageHandlerContext context)
        {
            return Configuration.Environment == Environment.Localhost
                ? Task.CompletedTask
                : Telegram.SendTextMessageAsync(new ChatId(message.ChatId), message.Text, ParseMode.Html);
        }
    }
}