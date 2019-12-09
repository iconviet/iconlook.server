using System;
using System.Reactive.Linq;
using System.Reflection;
using Iconlook.Server;
using Iconlook.Service.Job.Blockchain;

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
            Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(x => Resolve<UpdateBlockchainJob>().RunAsync().ConfigureAwait(false));
        }
    }
}