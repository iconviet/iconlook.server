using Iconlook.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using ServiceStack;

namespace Iconlook.Service.Mon
{
    public class MonHostConfiguration : HttpHostConfiguration
    {
        public MonHostConfiguration() : base("Mon", 83)
        {
        }

        public override void Configure(IApplicationBuilder application, IHostEnvironment environment)
        {
            base.Configure(application, environment);
            application.UseServiceStack(new MonServiceStack(this));
        }
    }
}