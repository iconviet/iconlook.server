using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Hangfire;
using Iconlook.Service.Web.Pages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using Syncfusion.Licensing;

namespace Iconlook.Service.Web
{
    public class Program : Agiper.Server.Program
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
                services.AddSingleton<WeatherForecastService>();
                services.AddDataProtection()
                    .PersistKeysToFileSystem(new DirectoryInfo("/var/lib/dotnet"))
                    .SetApplicationName(Assembly.GetEntryAssembly().GetName().Name);
            };
            Configure = host => application =>
            {
                application.UseResponseCaching();
                application.UseResponseCompression();
                application.UseStaticFiles();
                application.UseRouting();
                application.UseEndpoints(endpoints =>
                {
                    endpoints.MapBlazorHub();
                    endpoints.MapFallbackToPage("/_host");
                });
                application.UseServiceStack(host);
                SyncfusionLicenseProvider.RegisterLicense("MTI1Mjk1QDMxMzcyZTMyMmUzMEhsKzRudFN1U00rd2");
            };
            await StartAsync(new WebHost(), "http://*:80/");
        }
    }
}