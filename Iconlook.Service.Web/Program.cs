using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Iconlook.Service.Web
{
    public class Program
    {
        public static void Main()
        {
            Host.CreateDefaultBuilder().ConfigureWebHostDefaults(x => x.UseStartup<Startup>()).Build().Run();
        }
    }
}