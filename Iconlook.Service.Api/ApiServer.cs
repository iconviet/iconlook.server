using System.Threading.Tasks;
using Agiper.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Iconlook.Service.Api
{
    public class ApiServer : ServerBase
    {
        public static Task Main()
        {
            var configuration = new ApiServerConfiguration();
            return StartAsync(configuration,
                b => b.ConfigureWebHostDefaults(x => x.UseStartup(configuration.GetType())));
        }
    }
}