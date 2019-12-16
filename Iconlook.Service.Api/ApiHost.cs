using System.Reflection;
using Agiper.Server;
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
            HangfireJobPersistence = HangfireJobPersistence.Redis;
        }

        public ApiHost(string name, Assembly assembly) : base(name, assembly)
        {
        }
    }
}