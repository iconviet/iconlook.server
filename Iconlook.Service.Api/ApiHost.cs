using System.Reflection;
using ServiceStack;
using ServiceStack.Api.Swagger;

namespace Iconlook.Service.Api
{
    public class ApiHost : ServiceHost
    {
        public ApiHost() : base("Api", typeof(ApiHost).Assembly)
        {
            EndpointCanMakeCallbackRequests = true;
        }

        public ApiHost(string name, Assembly assembly) : base(name, assembly)
        {
        }

        protected override void ConfigureFeature()
        {
            base.ConfigureFeature();
            Plugins.Add(new SwaggerFeature());
            GetPlugin<ServerEventsFeature>().LimitToAuthenticatedUsers = true;
        }
    }
}