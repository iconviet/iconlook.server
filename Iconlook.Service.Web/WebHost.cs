using System;
using System.Globalization;
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
                UseRedisReplica = true;
            }
        }

        public void ConfigureCulture(Container container)
        {
            var culture = CultureInfo.InvariantCulture;
            var current = culture.Clone() as CultureInfo;
            current.NumberFormat.CurrencySymbol = "$";
            current.NumberFormat.PercentPositivePattern = 1;
            current.NumberFormat.PercentNegativePattern = 1;
            CultureInfo.DefaultThreadCurrentCulture = current;
            CultureInfo.DefaultThreadCurrentUICulture = current;
        }

        public override void Configure(Container container)
        {
            base.Configure(container);
            ConfigureCulture(container);
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