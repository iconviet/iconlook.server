using Agiper.Server;
using ServiceStack.Api.Swagger;

namespace Iconlook.Service.Api
{
    public class ApiServiceStack : ServiceStackBase
    {
        protected override void ConfigureFeature()
        {
            base.ConfigureFeature();
            Plugins.Add(new SwaggerFeature());
        }

        public ApiServiceStack(HostConfiguration config) : base(config.HostName, typeof(ApiServiceStack).Assembly, config)
        {
        }
    }
}