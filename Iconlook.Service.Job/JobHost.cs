using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Iconlook.Service.Job.Workers;
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
                    Observable.FromAsync(() => Container.TryResolve<UpdateBlockWorker>().StartAsync()).Delay(TimeSpan.FromSeconds(1)).Repeat().Subscribe();
                    Observable.FromAsync(() => Container.TryResolve<UpdateChainWorker>().StartAsync()).Delay(TimeSpan.FromSeconds(1)).Repeat().Subscribe();
                    Observable.FromAsync(() => Container.TryResolve<UpdatePRepsWorker>().StartAsync()).Delay(TimeSpan.FromSeconds(1)).Repeat().Subscribe();
                    Observable.FromAsync(() => Container.TryResolve<UpdateChainalyticWorker>().StartAsync()).Delay(TimeSpan.FromSeconds(1)).Repeat().Subscribe();
                });
        }
    }
}