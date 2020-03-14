using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Service.Job;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Iconlook.Service.Mon
{
    public class MonServer : ServerBase
    {
        public static Task Main()
        {
            var configuration = new MonServerConfiguration();
            return StartAsync(configuration,
                b => b.ConfigureWebHostDefaults(x => x.UseStartup(configuration.GetType())),
                c =>
                {
                    Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(_ => Provider.GetService<UpdatePeersJob>().RunAsync());
                });
        }
    }
}