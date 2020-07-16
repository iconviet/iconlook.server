using System.Threading.Tasks;
using Iconviet.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Iconlook.Service.Web
{
    public class HeaderProcessorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly HostConfiguration _configuration;

        public HeaderProcessorMiddleware(RequestDelegate next, HostConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext http)
        {
            http.Response.OnStarting(x =>
            {
                var state = (HttpContext) x;
                state.Response.Headers["X-Powered-By"] = _configuration.EndpointUniquelyAddressableName.ToLower();
                return Task.CompletedTask;
            }, http);
            await _next(http);
        }
    }

    public static class HeaderProcessorMiddlewareExtensions
    {
        public static IApplicationBuilder UseHeaderProcessor(this IApplicationBuilder builder, HostConfiguration configuration)
        {
            return builder.UseMiddleware<HeaderProcessorMiddleware>(configuration);
        }
    }
}