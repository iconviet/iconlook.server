using Iconviet.Server;
using ServiceStack.Api.OpenApi;

namespace Iconlook.Service.Api
{
    public class ApiServiceStack : ServiceStackBase
    {
        protected override void ConfigureFeature()
        {
            base.ConfigureFeature();
            Plugins.Add(new OpenApiFeature());
        }

        public ApiServiceStack(HostConfiguration config) : base(config.ServiceName, typeof(ApiServiceStack).Assembly, config)
        {
        }
    }
}