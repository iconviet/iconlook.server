using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Iconlook.Service.Web
{
    public class WebHost : HostBase
    {
        public static Task Main()
        {
            var cu = typeof(WebHost).Assembly.InformationVersion();
            var configuration = new WebHostConfiguration();
            return StartAsync(configuration, b => b.ConfigureWebHostDefaults(x => x.UseStaticWebAssets().UseStartup(configuration.GetType())));
        }
    }
}