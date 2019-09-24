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
            BackgroundJob.Enqueue<BlockProductionJob>(x => x.Run());
            RecurringJob.AddOrUpdate<BlockProductionJob>(x => x.Run(), "*/30 * * * * *");
        }

        protected override void ConfigureNServiceBusSqlServerTransportRouting<T>(RoutingSettings<T> routing)
        {
            base.ConfigureNServiceBusSqlServerTransportRouting(routing);
            routing.RegisterPublisher(typeof(JobHost).Assembly, EndpointName);
        }
    }
}