using Iconlook.Server;
using Microsoft.AspNetCore.Builder;
using ServiceStack;

namespace Iconlook.Service.Mon
{
    public class MonServerConfiguration : HttpServerConfiguration
    {
        public MonServerConfiguration() : base("Mon", 83)
        {
        }

        public override void Configure(IApplicationBuilder application)
        {
            base.Configure(application);
            application.UseServiceStack(new MonServiceHost(this));
        }
    }
}