using Iconlook.Server;
using ServiceStack.Api.Swagger;
using System.Reflection;

namespace Iconlook.Service.Api
{
    public class ApiHost : Host
    {
        protected override void ConfigureFeature()
        {
            base.ConfigureFeature();
            Plugins.Add(new SwaggerFeature());
        }

        public ApiHost() : base("Api", typeof(ApiHost).Assembly)
        {
        }

        public ApiHost(string name, Assembly assembly) : base(name, assembly)
        {
        }
    }
}