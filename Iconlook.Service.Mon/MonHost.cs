using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Service.Mon.Works;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Iconlook.Service.Mon
{
    public class MonHost : HostBase
    {
        public static Task Main()
        {
            var configuration = new MonHostConfiguration();
            return StartAsync(configuration,
                b => b.ConfigureWebHostDefaults(x => x.UseStartup(configuration.GetType())),
                c =>
                {
                    Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(async x => await Container.GetService<UpdatePeersWork>().StartAsync());
                });
        }
    }
}