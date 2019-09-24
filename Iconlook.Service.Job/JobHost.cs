using System.Reflection;
using Hangfire;
using Iconlook.Server;
using Iconlook.Service.Job.Block;
using Iconlook.Service.Job.Prep;
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
            RecurringJob.AddOrUpdate<VoteHistoryPrepJob>(x => x.Run(), Cron.Minutely());
            RecurringJob.AddOrUpdate<BlockProductionJob>(x => x.Run(), Cron.Minutely());
        }

        protected override void ConfigureNServiceBusSqlServerTransportRouting<T>(RoutingSettings<T> routing)
        {
            base.ConfigureNServiceBusSqlServerTransportRouting(routing);
            routing.RegisterPublisher(typeof(JobHost).Assembly, EndpointName);
        }
    }
}