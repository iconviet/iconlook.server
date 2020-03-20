using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Iconlook.Client;
using Iconlook.Message;
using NServiceBus;
using ServiceStack;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UAParser;

namespace Iconlook.Service.Job.Handlers
{
    public class WebRequestedEventHandler : HandlerBase, IHandleMessages<WebRequestedEvent>
    {
        public TelegramClient Telegram { get; set; }
        public HostConfiguration Configuration { get; set; }

        public Task Handle(WebRequestedEvent message, IMessageHandlerContext context)
        {
            var user_agent = Parser.GetDefault().Parse(message.UserAgent);
            var html = $"<b>{message.BodyString}</b> {message.IconString}\n" +
                       $"<pre>Hash-ID: {message.UserHashId.SafeSubstring(0, 4)} (friendly)</pre>\n" +
                       (message.Country.HasValue() ? $"<pre>Country: {new RegionInfo(message.Country).DisplayName}</pre>\n" : string.Empty) +
                       (message.Address.HasValue() ? $"<pre>Address: {message.Address}</pre>\n" : string.Empty) +
                       $"<pre>Request: {message.Url}</pre>\n" +
                       (message.Referer.HasValue() ? $"<pre>Referer: {message.Referer}</pre>\n" : string.Empty) +
                       $"<pre>Browser: {user_agent.Device}, {user_agent.OS}, {user_agent.UA.Family}</pre>";
            if (!message.Url.Contains("apple-touch-icon") &&
                (message.Url.StartsWith("https://iconlook.io") ||
                 message.Url.StartsWith("https://www.iconlook.io") &&
                 (!new[] { "other" }.Any(user_agent.OS.ToString().ToLower().Contains) ||
                  !new[] { "other", "bot" }.Any(user_agent.UA.Family.ToLower().Contains) ||
                  !new[] { "other", "spider" }.Any(user_agent.Device.ToString().ToLower().Contains))))
            {
                return Configuration.Environment == Environment.Localhost
                    ? Task.CompletedTask
                    : Telegram.SendTextMessageAsync(new ChatId(-1001449380420), html, ParseMode.Html);
            }
            return Task.CompletedTask;
        }
    }
}