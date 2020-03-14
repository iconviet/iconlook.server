using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Agiper.Server;
using Iconlook.Service.Job;
using Microsoft.Extensions.DependencyInjection;

namespace Iconlook.Service.Mon
{
    public class MonServer : ServerBase
    {
        public static Task Main()
        {
            return StartAsync(new MonServerConfiguration(), c =>
            {
                Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(_ => Provider.GetService<UpdatePeersJob>().RunAsync());
            });
        }
    }
}