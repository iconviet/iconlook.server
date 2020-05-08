using System;
using System.Threading.Tasks;
using Iconlook.Message;
using Iconviet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using NServiceBus;
using ServiceStack;

namespace Iconlook.Service.Web
{
    public class CookieProcessorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IApplicationBuilder _application;

        public CookieProcessorMiddleware(RequestDelegate next, IApplicationBuilder application)
        {
            _next = next;
            _application = application;
        }

        public async Task InvokeAsync(HttpContext http)
        {
            var new_user_hash_id = string.Empty;
            var url = http.Request.GetDisplayUrl();
            var referer = http.Request.Headers["Referer"].ToString();
            var country = http.Request.Headers["CF-IPCountry"].ToString();
            var user_agent = http.Request.Headers["User-Agent"].ToString();
            var address = http.Request.Headers["CF-Connecting-IP"].ToString();
            var old_user_hash_id = http.Request.Cookies[Cookies.USER_HASH_ID];
            http.Response.OnStarting(x =>
            {
                var state = (HttpContext) x;
                if (!state.Request.Path.StartsWithSegments("/_blazor"))
                {
                    if (old_user_hash_id.IsNullOrEmpty())
                    {
                        new_user_hash_id = Generate.HashId(16).ToUpper();
                        state.Response.Cookies.Append(
                            Cookies.USER_HASH_ID, new_user_hash_id, new CookieOptions
                            {
                                Expires = DateTime.UtcNow.AddYears(1)
                            });
                    }
                }
                return Task.CompletedTask;
            }, http);
            http.Response.OnCompleted(x =>
            {
                var state = (HttpContext) x;
                if (!state.Request.Path.StartsWithSegments("/_blazor"))
                {
                    var endpoint = _application.ApplicationServices.TryResolve<IMessageSession>();
                    if (old_user_hash_id.HasValue())
                    {
                        endpoint.Publish(new WebRequestedEvent
                        {
                            Url = url,
                            Referer = referer,
                            Address = address,
                            Country = country,
                            IconString = "🔸",
                            UserAgent = user_agent,
                            UserHashId = old_user_hash_id,
                            BodyString = "OLD USER REVISITED"
                        }).ConfigureAwait(false);
                    }
                    if (new_user_hash_id.HasValue())
                    {
                        endpoint.Publish(new WebRequestedEvent
                        {
                            Url = url,
                            Referer = referer,
                            IconString = "💠",
                            UserAgent = user_agent,
                            UserHashId = new_user_hash_id,
                            BodyString = "NEW USER DETECTED"
                        }).ConfigureAwait(false);
                    }
                }
                return Task.CompletedTask;
            }, http);
            await _next(http);
        }
    }

    public static class CookieProcessorMiddlewareExtensions
    {
        public static IApplicationBuilder UseCookieProcessor(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CookieProcessorMiddleware>(builder);
        }
    }
}