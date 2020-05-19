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
                    Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(async x => await Container.TryResolve<UpdateBlockWorker>().StartAsync());
                    Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(async x => await Container.TryResolve<UpdateChainWorker>().StartAsync());
                    Observable.Interval(TimeSpan.FromMinutes(1)).Subscribe(async x => await Container.TryResolve<UpdatePRepsWorker>().StartAsync());
                    Observable.Interval(TimeSpan.FromMinutes(1)).Subscribe(async x => await Container.TryResolve<UpdateChainalyticWorker>().StartAsync());
                });
        }
    }
}