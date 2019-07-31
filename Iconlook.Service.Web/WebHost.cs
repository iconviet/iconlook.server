using System;
using Agiper.Server;
using Funq;
using Iconlook.Service.Api;
using ServiceStack;
using Environment = Agiper.Environment;

namespace Iconlook.Service.Web
{
    public class WebHost : ApiHost
    {
        public WebHost() : base("Web", typeof(WebHost).Assembly)
        {
            TestMode = true;
            EndpointCanMakeCallbackRequests = true;
            if (Environment == Environment.Localhost)
            {
                NServiceBusTransport = NServiceBusTransport.Learning;
                NServiceBusPersistence = NServiceBusPersistence.Memory;
            }
        }

        public override void Configure(Container container)
        {
            base.Configure(container);
            RegisterServicesInAssembly(typeof(ApiHost).Assembly);
        }

        protected override void ConfigureFeature()
        {
            base.ConfigureFeature();
            GetPlugin<ServerEventsFeature>().LimitToAuthenticatedUsers = false;
        }

        public override RouteAttribute[] GetRouteAttributes(Type type)
        {
            var routes = base.GetRouteAttributes(type);
            if (type.Namespace == "Iconlook.Object" && type.Name.EndsWith("Request"))
            {
                routes.Each(x => x.Path = "/api" + x.Path);
            }
            return routes;
        }
    }
}