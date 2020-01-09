using System;
using System.Reactive.Linq;
using System.Reflection;
using Iconlook.Server;
using Iconlook.Service.Job;

namespace Iconlook.Service.Mon
{
    public class MonHost : Host
    {
        public MonHost() : base("Mon", typeof(MonHost).Assembly)
        {
        }

        public MonHost(string name, Assembly assembly) : base(name, assembly)
        {
        }

        protected override void OnStart()
        {
            Observable.FromAsync(() => Resolve<UpdatePeersJob>().RunAsync()).Delay(TimeSpan.FromSeconds(1)).Repeat().Throttle(TimeSpan.FromSeconds(2)).Subscribe();
        }
    }
}