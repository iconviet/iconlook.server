using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Agiper;
using Iconlook.Message;
using Iconlook.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using ServiceStack;
using StackExchange.Redis;
using Syncfusion.EJ2.Blazor;
using Syncfusion.Licensing;
using WebMarkupMin.AspNetCore3;
using Environment = Agiper.Environment;
using OperatingSystem = Agiper.OperatingSystem;

namespace Iconlook.Service.Web
{
    public class WebHostConfiguration : HttpHostConfiguration
    {
        public WebHostConfiguration() : base("Web", 80)
        {
            if (Environment != Environment.Localhost)
            {
                UseRedisReplica = true;
            }
        }

        public override void Configure(IApplicationBuilder application, IHostEnvironment environment)
        {
            base.Configure(application, environment);
            application.UseForwardedHeaders();
            application.UseStaticFiles();
            application.Use((http, next) =>
            {
                if (Environment != Environment.Localhost)
                {
                    http.Request.Scheme = "https";
                }
                return next();
            });
            application.Use((http, next) =>
            {
                var new_user_hash_id = string.Empty;
                var url = http.Request.GetDisplayUrl();
                var referer = http.Request.Headers["Referer"].ToString();
                var user_agent = http.Request.Headers["User-Agent"].ToString();
                var old_user_hash_id = http.Request.Cookies[Cookies.USER_HASH_ID];
                http.Response.OnStarting(x =>
                {
                    var state = (HttpContext) x;
                    if (!state.Request.Path.StartsWithSegments("/api") &&
                        !state.Request.Path.StartsWithSegments("/sse") &&
                        !state.Request.Path.StartsWithSegments("/_blazor"))
                    {
                        if (old_user_hash_id.IsNullOrEmpty())
                        {
                            new_user_hash_id = Generate.HashId(16).ToUpper();
                            state.Response.Cookies.Append(
                                Cookies.USER_HASH_ID, new_user_hash_id, new CookieOptions
                                {
                                    Expires = DateTime.UtcNow.AddMonths(1)
                                });
                        }
                    }
                    return Task.CompletedTask;
                }, http);
                http.Response.OnCompleted(x =>
                {
                    var state = (HttpContext) x;
                    if (!state.Request.Path.StartsWithSegments("/api") &&
                        !state.Request.Path.StartsWithSegments("/sse") &&
                        !state.Request.Path.StartsWithSegments("/_blazor"))
                    {
                        var endpoint = application.ApplicationServices.TryResolve<IMessageSession>();
                        if (old_user_hash_id.HasValue())
                        {
                            endpoint.Publish(new WebAccessedEvent
                            {
                                Url = url,
                                Referer = referer,
                                IconString = "🔸",
                                UserAgent = user_agent,
                                UserHashId = old_user_hash_id,
                                BodyString = "OLD USER REVISITED"
                            }).ConfigureAwait(false);
                        }
                        if (new_user_hash_id.HasValue())
                        {
                            endpoint.Publish(new WebAccessedEvent
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
                return next();
            });
            application.Use((http, next) =>
            {
                http.Response.OnStarting(x =>
                {
                    var state = (HttpContext) x;
                    var hostname = System.Environment.GetEnvironmentVariable("HOSTNAME");
                    state.Response.Headers["X-Powered-By"] =
                        (hostname.HasValue() ? $"{hostname}.{EndpointName}" : EndpointInstanceId).ToLower();
                    return Task.CompletedTask;
                }, http);
                return next();
            });
            application.Use((http, next) =>
            {
                if (http.Request.Path.StartsWithSegments("/api"))
                {
                    var query = QueryHelpers.ParseQuery(http.Request.QueryString.Value);
                    var builder = new QueryBuilder(query.SelectMany(x => x.Value, (x, y) =>
                        new KeyValuePair<string, string>(x.Key.Replace("$", string.Empty).Replace("top", "take"), y)));
                    http.Request.QueryString = builder.ToQueryString();
                }
                return next();
            });
            application.UseWhen(
                context => context.Request.Path.StartsWithSegments("/api") || context.Request.Path.StartsWithSegments("/sse"),
                builder => builder.UseServiceStack(new WebServiceStack(this)));
            application.UseRouting();
            application.UseWebMarkupMin();
            application.UseEndpoints(x =>
            {
                x.MapBlazorHub();
                x.MapFallbackToPage("/_Host");
            });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddSyncfusionBlazor();
            services.AddHttpContextAccessor();
            services.AddScoped<HttpContextAccessor>();
            services.Configure<KestrelServerOptions>(x =>
            {
                x.AllowSynchronousIO = true;
            });
            services.Configure<HubOptions>(x =>
            {
                x.EnableDetailedErrors = true;
                x.MaximumReceiveMessageSize = 1024 * 1024;
            });
            services.Configure<ForwardedHeadersOptions>(x =>
            {
                x.KnownProxies.Clear();
                x.KnownNetworks.Clear();
                x.ForwardedHeaders = ForwardedHeaders.All;
            });
            services.AddServerSideBlazor().AddCircuitOptions(x =>
            {
                if (Environment != Environment.Production)
                {
                    x.DetailedErrors = true;
                }
            });
            services.AddWebMarkupMin(x =>
            {
                x.DisablePoweredByHttpHeaders = true;
                x.AllowMinificationInDevelopmentEnvironment = true;
            }).AddHtmlMinification(x => x.MinificationSettings.RemoveHtmlComments = false);
            var connection = $"{Configuration.GetConnectionString("redis")},password={ServicePassword}";
            if (connection.HasValue())
            {
                if (Environment == Environment.Localhost)
                {
                    connection = connection.Replace("redis", "localhost");
                }
                services.AddSignalR().AddMessagePackProtocol().AddStackExchangeRedis(connection);
            }
            if (!OperatingSystem.IsWindows)
            {
                services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo("/var/lib/dotnet"))
                    .SetApplicationName(Assembly.GetEntryAssembly().GetName().Name);
            }
            else
            {
                services.AddDataProtection()
                    .SetApplicationName(Assembly.GetEntryAssembly().GetName().Name)
                    .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(connection), ProjectName);
            }
            services.AddRazorPages(x => x.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));
            SyncfusionLicenseProvider.RegisterLicense("MTgzMzE3QDMxMzcyZTM0MmUzMGsxUUk0Rkp6Vk9zaGNsaWlIQVBRWlhhNEpYa0hNRlFOWUVYUFBtcFFIWDg9");
        }
    }
}