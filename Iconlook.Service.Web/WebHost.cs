using System.Threading.Tasks;
using Agiper.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Iconlook.Service.Web
{
    public class WebHost : HostBase
    {
        public static Task Main()
        {
            var configuration = new WebHostConfiguration();
            return StartAsync(configuration, b => b.ConfigureWebHostDefaults(x => x.UseStaticWebAssets().UseStartup(configuration.GetType())));
        }
    }
}