using System.Reflection;
using Agiper.Server;
using Autofac;
using Iconlook.Client;
using Iconlook.Client.Binance;
using Iconlook.Client.Chainalytic;
using Iconlook.Client.Service;
using Iconlook.Client.Tracker;
using Iconlook.Message;
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

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterType<BinanceClient>();
            builder.RegisterType<TelegramClient>();
            builder.RegisterType<JsonHttpClient>();
            builder.RegisterType<ChainalyticClient>();
            builder.RegisterType<IconTrackerClient>();
            builder.RegisterType<IconServiceClient>();
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
            routing.RouteToEndpoint(typeof(SendTelegramCommand), "Iconlook.Job");
        }
    }
}