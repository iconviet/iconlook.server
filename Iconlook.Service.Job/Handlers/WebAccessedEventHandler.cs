using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Iconlook.Client;
using Iconlook.Message;
using NServiceBus;
using Serilog;
using ServiceStack;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UAParser;

namespace Iconlook.Service.Job.Handlers
{
    public class WebAccessedEventHandler : HandlerBase, IHandleMessages<WebAccessedEvent>
    {
        public TelegramClient Telegram { get; set; }
        public HostConfiguration Configuration { get; set; }

        public Task Handle(WebAccessedEvent message, IMessageHandlerContext context)
        {
            var user_agent = Parser.GetDefault().Parse(message.UserAgent);
            var html = $"<b>{message.BodyString}</b> {message.IconString}\n" +
                       $"<pre>Hash-ID: {message.UserHashId.SafeSubstring(0, 4)} (friendly)</pre>\n" +
                       (message.Address.HasValue() ? $"<pre>Address: {message.Address}</pre>\n" : string.Empty) +
                       $"<pre>Request: {message.Url}</pre>\n" +
                       (message.Referer.HasValue() ? $"<pre>Referer: {message.Referer}</pre>\n" : string.Empty) +
                       $"<pre>Browser: {user_agent.Device}, {user_agent.OS}, {user_agent.UA.Family}</pre>";
            Log.Information(html);
            return Configuration.Environment == Environment.Localhost
                ? Task.CompletedTask
                : Telegram.SendTextMessageAsync(new ChatId(-1001449380420), html, ParseMode.Html);
        }
    }
}