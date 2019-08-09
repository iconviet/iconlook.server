using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Hangfire;
using Iconlook.Service.Api;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using Syncfusion.Licensing;

namespace Iconlook.Service.Web
{
    public class Program : ProgramBase
    {
        public static async Task Main()
        {
            Configure = host => application =>
            {
                application.UseForwardedHeaders();
                application.UseResponseCompression();
                application.UseStaticFiles();
                application.UseCookiePolicy();
                application.UseRouting();
                application.UseEndpoints(x =>
                {
                    x.MapBlazorHub();
                    x.MapFallbackToPage("/_Host");
                });
                application.UseServiceStack(host);
            };
            ConfigureServices = host => services =>
            {
                services.AddRazorPages();
                services.AddHangfire(x => { });
                services.AddServerSideBlazor();
                services.AddResponseCompression();
                services.AddSingleton<PrepService>();
                services.AddSingleton<TransactionService>();
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.KnownProxies.Clear();
                    options.KnownNetworks.Clear();
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedHost;
                });
                services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo("/var/lib/dotnet"))
                    .SetApplicationName(Assembly.GetEntryAssembly().GetName().Name);
                var redis = host.HostConfiguration.GetConnectionString("redis");
                if (redis.HasValue())
                {
                    if (host.Environment == Environment.Localhost)
                    {
                        redis = redis.Replace("redis", "localhost");
                    }
                    services.AddSignalR().AddMessagePackProtocol().AddStackExchangeRedis(redis);
                }
            };
            SyncfusionLicenseProvider.RegisterLicense("MTI1OTM0QDMxMzcyZTMyMmUzMG0yUm01UnZ6U3pQMj" +
                                                      "dLdEM1Q3RSSE1YdHl2R0RmQWh2N0JuZEZrd1BTc2s9");
            await StartAsync(new WebHost(), 80);
        }
    }
}