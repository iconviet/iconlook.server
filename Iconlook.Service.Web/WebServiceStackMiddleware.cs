using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iconviet.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using ServiceStack;

namespace Iconlook.Service.Web
{
    public class WebServiceStackMiddleware
    {
        private readonly RequestDelegate _next;

        public WebServiceStackMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext http)
        {
            if (http.Request.Path.StartsWithSegments("/api"))
            {
                var query = QueryHelpers.ParseQuery(http.Request.QueryString.Value);
                var builder = new QueryBuilder(query.SelectMany(x => x.Value, (x, y) =>
                    new KeyValuePair<string, string>(x.Key.Replace("$", string.Empty).Replace("top", "take"), y)));
                http.Request.QueryString = builder.ToQueryString();
            }
            await _next(http);
        }
    }

    public static class WebServiceStackMiddlewareExtensions
    {
        public static IApplicationBuilder UseWebServiceStack(this IApplicationBuilder application, HostConfiguration configuration)
        {
            return application.UseMiddleware<WebServiceStackMiddleware>().UseServiceStack(new WebServiceStack(configuration));
        }
    }
}