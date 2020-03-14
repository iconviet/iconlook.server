using System.Threading.Tasks;
using Agiper.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Iconlook.Service.Web
{
    public class WebServer : ServerBase
    {
        public static Task Main()
        {
            var configuration = new WebServerConfiguration();
            return StartAsync(configuration, b => b.ConfigureWebHostDefaults(x => x.UseStartup(configuration.GetType())));
        }
    }
}