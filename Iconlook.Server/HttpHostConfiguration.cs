using System.Reflection;
using Autofac;
using Iconlook.Shared;
using Iconlook.Shared.Binance;
using Iconlook.Shared.Chainalytic;
using Iconlook.Shared.Service;
using Iconlook.Shared.Tracker;
using Iconlook.Message;
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
        }

        protected override void ConfigureRavenDb(ContainerBuilder builder)
        {
        }

        protected override void ConfigureElastic(ContainerBuilder builder)
        {
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterType<JsonHttpClient>().PropertiesAutowired();
            builder.RegisterType<BinanceApiClient>().PropertiesAutowired();
            builder.RegisterType<TelegramApiClient>().PropertiesAutowired();
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
            routing.RouteToEndpoint(typeof(TextMessageCommand), $"{ProjectName}.Job");
        }
    }
}