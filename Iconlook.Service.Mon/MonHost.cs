using System;
using System.Reactive.Linq;
using System.Reflection;
using Iconlook.Server;

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
            Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(async x => await Resolve<UpdatePeerJob>().RunAsync());

        }
    }
}