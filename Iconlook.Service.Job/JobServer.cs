using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Microsoft.Extensions.DependencyInjection;

namespace Iconlook.Service.Job
{
    public class JobServer : ServerBase
    {
        public static Task Main()
        {
            return StartAsync(new JobServerConfiguration(), c =>
            {
                Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(async x => await Provider.GetService<UpdateBlockJob>().RunAsync());
                Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(async x => await Provider.GetService<UpdateChainJob>().RunAsync());
                Observable.Interval(TimeSpan.FromMinutes(1)).Subscribe(async x => await Provider.GetService<UpdatePRepsJob>().RunAsync());
                Observable.Interval(TimeSpan.FromMinutes(1)).Subscribe(async x => await Provider.GetService<UpdateUnstakingJob>().RunAsync());
            });
        }
    }
}