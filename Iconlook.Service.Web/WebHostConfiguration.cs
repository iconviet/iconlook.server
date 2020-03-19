using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Agiper;
using Iconlook.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceStack;
using StackExchange.Redis;
using Syncfusion.Blazor;
using Syncfusion.Licensing;
using WebMarkupMin.AspNetCore3;

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
            application.UseHeaderProcessor(this);
            application.Use((http, next) =>
            {
                if (Environment != Environment.Localhost)
                {
                    http.Request.Scheme = "https";
                }
                return next();
            });
            application.UseStaticFiles();
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
            application.UseCookieProcessor(); // DO NOT MOVE THIS !!!!
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
            SyncfusionLicenseProvider.RegisterLicense("MjI0ODk5QDMxMzgyZTMxMmUzMGYrRTdlbUhkc0xDMXFMdlhiaDk3ZUtPVlN3c3lzQm9XeUNUdVVCQXhxNWc9");
        }
    }
}