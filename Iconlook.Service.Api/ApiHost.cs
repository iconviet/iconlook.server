using System.Reflection;
using Agiper;
using Iconlook.Server;
using ServiceStack.Api.Swagger;

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
            if (Environment != Environment.Localhost)
            {
                Hosts["redis"] = "localhost";
            }
        }

        public ApiHost(string name, Assembly assembly) : base(name, assembly)
        {
        }
    }
}