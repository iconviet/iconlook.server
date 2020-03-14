using System;
using System.Globalization;
using Agiper.Server;
using Funq;
using Iconlook.Service.Api;
using ServiceStack;

namespace Iconlook.Service.Web
{
    public class WebServiceHost : ServiceHostBase
    {
        public WebServiceHost(ServerConfiguration config) : base(config.HostName, typeof(WebServiceHost).Assembly, config)
        {
        }

        public void ConfigureDefaultCultures()
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
            ConfigureDefaultCultures();
            Config.EnableFeatures = Feature.Json;
            SetConfig(new HostConfig { UseCamelCase = false });
            RegisterServicesInAssembly(typeof(ApiServiceHost).Assembly);
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