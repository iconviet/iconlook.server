using System;
using System.Reactive.Linq;
using System.Reflection;
using Iconlook.Server;

namespace Iconlook.Service.Job
{
    public class JobHost : Host
    {
        public JobHost() : base("Job", typeof(JobHost).Assembly)
        {
        }

        public JobHost(string name, Assembly assembly) : base(name, assembly)
        {
        }

        protected override void OnStart()
        {
            Observable.Interval(TimeSpan.FromSeconds(9)).Subscribe(async x => await Resolve<UpdatePRepsJob>().RunAsync());
            Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(async x => await Resolve<UpdateBlockchainJob>().RunAsync());
        }
    }
}