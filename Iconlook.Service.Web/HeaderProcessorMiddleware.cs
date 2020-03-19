using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Environment = System.Environment;

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
                var hostname = Environment.GetEnvironmentVariable("HOSTNAME");
                state.Response.Headers["X-Powered-By"] =
                    (hostname.HasValue() ? $"{hostname}.{_configuration.EndpointName}" : _configuration.EndpointInstanceId).ToLower();
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