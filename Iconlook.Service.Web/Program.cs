using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using Syncfusion.Licensing;
using WebMarkupMin.AspNetCore3;

namespace Iconlook.Service.Web
{
    public class Program : ProgramBase
    {
        public static async Task Main()
        {
            ConfigureServices = host => services =>
            {
                services.AddRazorPages();
                services.AddHangfire(x => { });
                services.AddServerSideBlazor();
                services.AddResponseCompression();
                services.Configure<KestrelServerOptions>(options =>
                {
                    options.AllowSynchronousIO = true;
                });
                services.Configure<HubOptions>(options =>
                {
                    options.EnableDetailedErrors = true;
                    options.MaximumReceiveMessageSize = 1024 * 1024;
                });
                services.AddWebMarkupMin(options =>
                {
                    options.AllowMinificationInDevelopmentEnvironment = true;
                }).AddHtmlMinification(options =>
                {
                    options.MinificationSettings.RemoveHtmlComments = false;
                });
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.KnownProxies.Clear();
                    options.KnownNetworks.Clear();
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedHost;
                });
                if (!OperatingSystem.IsWindows)
                {
                    services.AddDataProtection()
                        .PersistKeysToFileSystem(new DirectoryInfo("/var/lib/dotnet"))
                        .SetApplicationName(Assembly.GetEntryAssembly().GetName().Name);
                }
                var connectionstring = host.HostConfiguration.GetConnectionString("redis");
                if (connectionstring.HasValue())
                {
                    if (host.Environment == Environment.Localhost)
                    {
                        connectionstring = connectionstring.Replace("redis", "localhost");
                    }
                    services.AddSignalR().AddMessagePackProtocol().AddStackExchangeRedis(connectionstring);
                }
            };
            Configure = host => application =>
            {
                if (host.Environment != Environment.Localhost)
                {
                    application.Use((context, next) =>
                    {
                        context.Request.Scheme = "https";
                        return next();
                    });
                }
                application.UseForwardedHeaders();
                application.UseResponseCompression();
                application.UseStaticFiles();
                application.UseWhen(c => c.Request.Path.StartsWithSegments("/api"), a => a.UseServiceStack(host));
                application.UseRouting();
                application.UseWebMarkupMin();
                application.UseEndpoints(x =>
                {
                    x.MapBlazorHub();
                    x.MapFallbackToPage("/_Page");
                });
            };
            SyncfusionLicenseProvider.RegisterLicense("MTI1OTM0QDMxMzcyZTMyMmUzMG0yUm01UnZ6U3pQMjdLdEM1Q3RSSE1YdHl2R0RmQWh2N0JuZEZrd1BTc2s9");
            await StartAsync(new WebHost(), 80);
        }
    }
}