using System;
using System.Threading.Tasks;
using Iconlook.Message;
using Iconviet;
using Iconviet.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using NServiceBus;
using ServiceStack;
using Environment = System.Environment;

namespace Iconlook.Service.Web
{
    public class CookieProcessorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HostConfiguration _configuration;
        private readonly IApplicationBuilder _application;

        public CookieProcessorMiddleware(RequestDelegate next, IApplicationBuilder application, HostConfiguration configuration)
        {
            _next = next;
            _application = application;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext http)
        {
            var url = http.Request.GetDisplayUrl();
            var referer = http.Request.Headers["Referer"].ToString();
            var country = http.Request.Headers["CF-IPCountry"].ToString();
            var user_hash_id = http.Request.Cookies[Cookies.USER_HASH_ID];
            var user_agent = http.Request.Headers["User-Agent"].ToString();
            var address = http.Request.Headers["CF-Connecting-IP"].ToString();
            var x_powered_by = _configuration.EndpointUniquelyAddressableName.ToLower();
            http.Response.OnStarting(x =>
            {
                var state = (HttpContext) x;
                if (!state.Request.Path.StartsWithSegments("/_blazor"))
                {
                    var web_requested_event = new WebRequestedEvent
                    {
                        Url = url,
                        Referer = referer,
                        Address = address,
                        Country = country,
                        UserAgent = user_agent,
                        UserHashId = user_hash_id,
                        XPoweredBy = x_powered_by
                    };
                    var endpoint = _application.ApplicationServices.TryResolve<IMessageSession>();
                    if (user_hash_id.HasValue())
                    {
                        endpoint.Publish(web_requested_event.ThenDo(e =>
                        {
                            e.IconString = "🔸";
                            e.BodyString = "OLD USER REVISITED";
                        })).ConfigureAwait(false);
                    }
                    else
                    {
                        user_hash_id = Generate.HashId(16).ToUpper();
                        endpoint.Publish(web_requested_event.ThenDo(e =>
                        {
                            e.IconString = "💠";
                            e.UserHashId = user_hash_id;
                            e.BodyString = "NEW USER DETECTED";
                        })).ConfigureAwait(false);
                        state.Response.Cookies.Append(
                            Cookies.USER_HASH_ID, user_hash_id, new CookieOptions
                            {
                                Expires = DateTime.UtcNow.AddYears(1)
                            });
                    }
                }
                return Task.CompletedTask;
            }, http);
            await _next(http);
        }
    }

    public static class CookieProcessorMiddlewareExtensions
    {
        public static IApplicationBuilder UseCookieProcessor(this IApplicationBuilder application, HostConfiguration configuration)
        {
            return application.UseMiddleware<CookieProcessorMiddleware>(application, configuration);
        }
    }
}