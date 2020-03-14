using System.Reflection;
using Agiper.Server;
using NServiceBus;

namespace Iconlook.Server
{
    public class HttpServerConfiguration : Agiper.Server.HttpServerConfiguration
    {
        public override string ProjectName => "Iconlook";

        public HttpServerConfiguration(string hostname, int port) : base(hostname, port)
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