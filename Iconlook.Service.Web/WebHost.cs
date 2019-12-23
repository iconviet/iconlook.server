using System;
using Funq;
using Iconlook.Server;
using Iconlook.Service.Api;
using ServiceStack;
using Environment = Agiper.Environment;

namespace Iconlook.Service.Web
{
    public class WebHost : Host
    {
        public WebHost() : base("Web", typeof(WebHost).Assembly)
        {
            if (Environment != Environment.Localhost)
            {
                Hosts["redis"] = "localhost";
            }
        }

        public override void Configure(Container container)
        {
            base.Configure(container);
            Config.EnableFeatures = Feature.Json;
            SetConfig(new HostConfig { UseCamelCase = false });
            RegisterServicesInAssembly(typeof(ApiHost).Assembly);
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