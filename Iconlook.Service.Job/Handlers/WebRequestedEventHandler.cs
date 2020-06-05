using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Iconlook.Client;
using Iconlook.Message;
using Iconviet;
using Iconviet.Server;
using NServiceBus;
using ServiceStack;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UAParser;

namespace Iconlook.Service.Job.Handlers
{
    public class WebRequestedEventHandler : HandlerBase, IHandleMessages<WebRequestedEvent>
    {
        public TelegramApiClient Telegram { get; set; }
        public HostConfiguration Configuration { get; set; }

        public Task Handle(WebRequestedEvent message, IMessageHandlerContext context)
        {
            var myself = new[] { "ODNB", "9B6N" };
            if (message.Country.HasValue() && !myself.Any(message.UserHashId.StartsWith))
            {
                var blacklist = new[] { "bot", "other", "spider" };
                var ua = Parser.GetDefault().Parse(message.UserAgent);
                var html = $"<b>{message.BodyString}</b> {message.IconString}\n" +
                           $"<pre>Hash-ID: {message.UserHashId.SafeSubstring(0, 4)}</pre>\n" +
                           $"<pre>Country: {new RegionInfo(message.Country).EnglishName}</pre>\n" +
                           (message.Address.HasValue() ? $"<pre>Address: {message.Address}</pre>\n" : string.Empty) +
                           $"<pre>Request: {message.Url}</pre>\n" +
                           (message.Referer.HasValue() ? $"<pre>Referer: {message.Referer}</pre>\n" : string.Empty) +
                           $"<pre>Machine: {message.XPoweredBy}</pre>\n" +
                           $"<pre>Browser: {ua.Device}, {ua.OS}, {ua.UA}</pre>";
                if (!message.Url.Contains("apple-touch-icon") &&
                    (message.Url.StartsWith("https://iconlook.io") ||
                     message.Url.StartsWith("https://www.iconlook.io")) &&
                    (!blacklist.Any(ua.OS.ToString().ToLower().Contains) ||
                     !blacklist.Any(ua.UA.ToString().ToLower().Contains) ||
                     !blacklist.Any(ua.Device.ToString().ToLower().Contains)))
                {
                    return Configuration.Environment == Environment.Localhost
                        ? Task.CompletedTask
                        : Telegram.SendTextMessageAsync(new ChatId(-1001449380420), html, ParseMode.Html);
                }
            }
            return Task.CompletedTask;
        }
    }
}