using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Agiper.Server;
using Hangfire;
using Iconlook.Service.Api;
using Iconlook.Service.Web.Pages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using Syncfusion.Licensing;

namespace Iconlook.Service.Web
{
    public class Program : ProgramBase
    {
        public static async Task Main()
        {
            ConfigureServices = host => services =>
            {
                services.AddRazorPages();
                services.AddHangfire(x => {});
                services.AddResponseCaching();
                services.AddServerSideBlazor();
                services.AddResponseCompression();
                services.Configure<ForwardedHeadersOptions>(x =>
                {
                    x.KnownProxies.Clear();
                    x.KnownNetworks.Clear();
                    x.ForwardedHeaders = ForwardedHeaders.All;
                });
                services.AddSingleton<PrepService>();
                services.AddSingleton<TransactionService>();
                services.AddSingleton<WeatherForecastService>();
                services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo("/var/lib/dotnet"))
                    .SetApplicationName(Assembly.GetEntryAssembly().GetName().Name);
            };
            Configure = host => application =>
            {
                application.UseResponseCaching();
                application.UseForwardedHeaders();
                application.UseResponseCompression();
                application.UseStaticFiles();
                application.UseRouting();
                application.UseEndpoints(x =>
                {
                    x.MapBlazorHub();
                    x.MapFallbackToPage("/_host");
                });
                application.UseServiceStack(host);
                SyncfusionLicenseProvider.RegisterLicense("MTI1OTM0QDMxMzcyZTMyMmUzMG0yUm01UnZ6U3pQMj" +
                                                          "dLdEM1Q3RSSE1YdHl2R0RmQWh2N0JuZEZrd1BTc2s9");
            };
            await StartAsync(new WebHost(), 80);
        }
    }
}