using Agiper.Server;
using ServiceStack;
using ServiceStack.Api.Swagger;

namespace Iconlook.Service.Api
{
    public class ApiServiceHost : ServiceHostBase
    {
        protected override void ConfigureFeature()
        {
            base.ConfigureFeature();
            Plugins.Add(new SwaggerFeature());
            GetPlugin<ServerEventsFeature>().LimitToAuthenticatedUsers = false;
        }

        public ApiServiceHost(ServerConfiguration config) : base(config.HostName, typeof(ApiServiceHost).Assembly, config)
        {
        }
    }
}