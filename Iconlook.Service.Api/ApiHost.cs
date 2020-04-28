using System.Threading.Tasks;
using Iconviet.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Iconlook.Service.Api
{
    public class ApiHost : HostBase
    {
        public static Task Main()
        {
            var configuration = new ApiHostConfiguration();
            return StartAsync(configuration,
                b => b.ConfigureWebHostDefaults(x => x.UseStartup(configuration.GetType())));
        }
    }
}