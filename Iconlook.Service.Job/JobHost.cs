using System;
using System.Reactive.Linq;
using System.Reflection;
using Hangfire;
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
            Observable.FromAsync(() => Resolve<UpdateChainJob>().RunAsync()).Repeat().Subscribe();
            RecurringJob.AddOrUpdate<UpdatePRepsJob>(x => x.RunAsync(), "*/2 * * * *", TimeZoneInfo.Utc, HangfireQueueName);
        }
    }
}