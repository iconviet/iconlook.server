using System.Reflection;
using Autofac;
using Iconlook.Common;
using Iconlook.Common.Binance;
using Iconlook.Common.Chainalytic;
using Iconlook.Common.Service;
using Iconlook.Common.Tracker;
using Iconlook.Message;
using Iconviet.Server;
using NServiceBus;

namespace Iconlook.Server
{
    public class HostConfiguration : Iconviet.Server.HostConfiguration
    {
        public override string ProjectName => "Iconlook";

        public HostConfiguration(string hostname) : base(hostname)
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