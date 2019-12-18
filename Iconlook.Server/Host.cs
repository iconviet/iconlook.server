using System.Reflection;
using Agiper.Server;
using NServiceBus;
using ServiceStack;

namespace Iconlook.Server
{
    public abstract class Host : HostBase
    {
        public override string ProjectName => "Iconlook";

        public static HostBase Current => Instance as HostBase;

        protected Host(string name, Assembly assembly) : base(name, assembly)
        {
            NServiceBusTransport = NServiceBusTransport.SqlServer;
            NServiceBusPersistence = NServiceBusPersistence.SqlServer;
            HangfireJobPersistence = HangfireJobPersistence.SqlServer;
        }

        protected override void ConfigureFeature()
        {
            base.ConfigureFeature();
            GetPlugin<ServerEventsFeature>().LimitToAuthenticatedUsers = false;
        }

        protected override void ConfigureNServiceBus(EndpointConfiguration configuration)
        {
            base.ConfigureNServiceBus(configuration);
            var validation = configuration.UseFluentValidation();
            validation.AddValidatorsFromAssembly(Assembly.Load("Iconlook.Message"));
        }
    }
}