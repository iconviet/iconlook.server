using System.Reflection;
using Autofac;
using Iconlook.Message;
using Iconlook.Shared;
using Iconviet;
using Iconviet.Server;
using NServiceBus;

namespace Iconlook.Server
{
    public class HttpHostConfiguration : Iconviet.Server.HttpHostConfiguration
    {
        public override string ProjectName => "Iconlook";

        public HttpHostConfiguration(string hostname, int port = 0) : base(hostname, port)
        {
            NServiceBusPurgeOnStartup = true;
            NServiceBusTransport = NServiceBusTransport.RabbitMQ;
            NServiceBusPersistence = NServiceBusPersistence.RavenDB;
            IconNetwork.CitizenUrl = "CITIZEN_URL".EnvironmentVariable();
            IconNetwork.TrackerUrl = "TRACKER_URL".EnvironmentVariable();
        }

        protected override void ConfigureRavenDb(ContainerBuilder builder)
        {
        }

        protected override void ConfigureElastic(ContainerBuilder builder)
        {
        }

        public override void ConfigureNServiceBus(EndpointConfiguration configuration)
        {
            base.ConfigureNServiceBus(configuration);
            var validation = configuration.UseFluentValidation();
            validation.AddValidatorsFromAssembly(Assembly.Load($"{ProjectName}.Message"));
        }

        protected override void ConfigureNServiceBusTransportRouting(RoutingSettings routing)
        {
            base.ConfigureNServiceBusTransportRouting(routing);
            routing.RouteToEndpoint(typeof(TextMessageCommand), $"{ProjectName}.Job");
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterInstance(new TelegramApiClient("892011336:AAHI0I6b3dDYuCej6RvijUYrZSJXgX4w5hw"));
        }
    }
}