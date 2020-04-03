using System.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NodaTime.TimeZones;

namespace Iconlook.Service.Web
{
    public class WebHost : HostBase
    {
        public static Task Main()
        {
            var cu = TzdbDateTimeZoneSource.Default.ZoneLocations.Single(x => x.CountryCode == "VN");
            var configuration = new WebHostConfiguration();
            return StartAsync(configuration, b => b.ConfigureWebHostDefaults(x => x.UseStaticWebAssets().UseStartup(configuration.GetType())));
        }
    }
}