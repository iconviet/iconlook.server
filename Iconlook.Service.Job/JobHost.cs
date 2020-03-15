using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
                    Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(async x => await Container.GetService<UpdateBlockJob>().RunAsync());
                    Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(async x => await Container.GetService<UpdateChainJob>().RunAsync());
                    Observable.Interval(TimeSpan.FromMinutes(1)).Subscribe(async x => await Container.GetService<UpdatePRepsJob>().RunAsync());
                    Observable.Interval(TimeSpan.FromMinutes(1)).Subscribe(async x => await Container.GetService<UpdateUnstakingJob>().RunAsync());
                });
        }
    }
}