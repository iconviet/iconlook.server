using System.IO;
using System.Linq;
using System.Reflection;
using Iconlook.Server;
using Iconviet;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using Syncfusion.Licensing;
using WebMarkupMin.AspNetCore3;

namespace Iconlook.Service.Web
{
    public class WebHostConfiguration : HttpHostConfiguration
    {
        public WebHostConfiguration() : base("Web", 80)
        {
            UseRedisReplica = Environment != Environment.Localhost;
        }

        public override void Configure(IApplicationBuilder application, IHostEnvironment environment)
        {
            base.Configure(application, environment);
            application
                .UseForwardedHeaders()
                .UseHeaderProcessor(this)
                .UseWhen(
                    context => !context.Request.Headers["CF-Request-ID"].Any(),
                    builder => builder.UseResponseCompression())
                .UseStaticFiles()
                .MapWhen(
                    context => context.Request.Path.StartsWithSegments("/api") ||
                               context.Request.Path.StartsWithSegments("/sse"),
                    builder => builder.UseWebServiceStack(this))
                .UseCookieProcessor()
                .UseRouting()
                .UseWebMarkupMin()
                .UseEndpoints(x =>
                {
                    x.MapBlazorHub();
                    x.MapFallbackToPage("/_Host");
                });
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services
                .AddServerSideBlazor();
            services
                .AddSyncfusionBlazor()
                .AddResponseCompression()
                .AddHttpContextAccessor()
                .AddScoped<HttpContextAccessor>()
                .Configure<ForwardedHeadersOptions>(x =>
                {
                    x.KnownProxies.Clear();
                    x.KnownNetworks.Clear();
                    x.ForwardedHeaders = ForwardedHeaders.All;
                })
                .AddWebMarkupMin(x =>
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