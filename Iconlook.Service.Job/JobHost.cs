using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Iconviet.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ServiceStack;

namespace Iconlook.Service.Job
{
    public class JobHost : HostBase
    {
        public static Task Main()
        {
            var configuration = new JobHostConfiguration();
            return StartAsync(configuration,
                b => b.ConfigureWebHostDefaults(x => x.UseStartup(configuration.GetType())),
                c =>
                {
                    Observable.FromAsync(() => Container.TryResolve<UpdateBlockJob>().StartAsync()).Delay(TimeSpan.FromSeconds(1)).Repeat().Subscribe();
                    Observable.FromAsync(() => Container.TryResolve<UpdateChainJob>().StartAsync()).Delay(TimeSpan.FromSeconds(1)).Repeat().Subscribe();
                    Observable.FromAsync(() => Container.TryResolve<UpdatePRepsJob>().StartAsync()).Delay(TimeSpan.FromSeconds(1)).Repeat().Subscribe();
                    Observable.FromAsync(() => Container.TryResolve<UpdateChainalyticJob>().StartAsync()).Delay(TimeSpan.FromSeconds(1)).Repeat().Subscribe();
                });
        }
    }
}