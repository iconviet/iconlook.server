using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Iconviet.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ServiceStack;

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
                    Observable.FromAsync(() => Container.TryResolve<UpdatePeersJob>().StartAsync()).Repeat().Subscribe();
                });
        }
    }
}