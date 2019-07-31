using System;
using Funq;
using Iconlook.Service.Api;
using ServiceStack;

namespace Iconlook.Service.Web
{
    public class WebHost : ApiHost
    {
        public WebHost() : base("Web", typeof(WebHost).Assembly)
        {
            TestMode = true;
            EndpointCanMakeCallbackRequests = true;
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