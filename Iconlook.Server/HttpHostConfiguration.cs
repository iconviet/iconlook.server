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
    public class HttpHostConfiguration : Agiper.Server.HttpHostConfiguration
    {
        public override string ProjectName => "Iconlook";

        public HttpHostConfiguration(string hostname, int port) : base(hostname, port)
        {
            NServiceBusPurgeOnStartup = true;
            NServiceBusTransport = NServiceBusTransport.RabbitMQ;
            HangfireJobPersistence = HangfireJobPersistence.Memory;
            NServiceBusPersistence = NServiceBusPersistence.SqlServer;
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterType<BinanceClient>().PropertiesAutowired();
            builder.RegisterType<TelegramClient>().PropertiesAutowired();
            builder.RegisterType<JsonHttpClient>().PropertiesAutowired();
            builder.RegisterType<ChainalyticClient>().PropertiesAutowired();
            builder.RegisterType<IconTrackerClient>().PropertiesAutowired();
            builder.RegisterType<IconServiceClient>().PropertiesAutowired();
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