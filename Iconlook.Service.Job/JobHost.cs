using System;
using System.Reactive.Linq;
using System.Reflection;
using Hangfire;
using Iconlook.Server;
using Iconlook.Service.Job.Blockchain;
using NServiceBus;

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
            Observable.Interval(TimeSpan.FromSeconds(2)).Subscribe(async x =>
            {
                await Resolve<UpdateBlockchainJob>().RunAsync();
            });
            RecurringJob.AddOrUpdate<UpdateBlockchainJob>(x => x.RunAsync(), "*/30 * * * * *");
        }

        protected override void ConfigureNServiceBusSqlServerTransportRouting<T>(RoutingSettings<T> routing)
        {
            base.ConfigureNServiceBusSqlServerTransportRouting(routing);
            routing.RegisterPublisher(typeof(JobHost).Assembly, EndpointName);
        }
    }
}