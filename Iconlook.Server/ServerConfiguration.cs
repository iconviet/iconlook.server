using System.Reflection;
using Agiper.Server;
using NServiceBus;

namespace Iconlook.Server
{
    public class ServerConfiguration : Agiper.Server.ServerConfiguration
    {
        public override string ProjectName => "Iconlook";

        public ServerConfiguration(string hostname) : base(hostname)
        {
            NServiceBusTransport = NServiceBusTransport.RabbitMQ;
            HangfireJobPersistence = HangfireJobPersistence.Memory;
            NServiceBusPersistence = NServiceBusPersistence.SqlServer;
        }

        public override void ConfigureNServiceBus(EndpointConfiguration configuration)
        {
            base.ConfigureNServiceBus(configuration);
            var validation = configuration.UseFluentValidation();
            validation.AddValidatorsFromAssembly(Assembly.Load($"{ProjectName}.Message"));
        }
    }
}