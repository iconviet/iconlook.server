using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Iconviet.Server;
using Iconlook.Service.Job.Works;
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
                    Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(async x => await Container.TryResolve<UpdateBlockWork>().StartAsync());
                    Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(async x => await Container.TryResolve<UpdateChainWork>().StartAsync());
                    Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(async x => await Container.TryResolve<UpdateMegaloopWork>().StartAsync());
                    Observable.Interval(TimeSpan.FromMinutes(1)).Subscribe(async x => await Container.TryResolve<UpdatePRepsWork>().StartAsync());
                    Observable.Interval(TimeSpan.FromMinutes(1)).Subscribe(async x => await Container.TryResolve<UpdateChainalyticWork>().StartAsync());
                });
        }
    }
}