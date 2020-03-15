using Agiper;
using Iconlook.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using ServiceStack;

namespace Iconlook.Service.Api
{
    public class ApiServerConfiguration : HttpServerConfiguration
    {
        public ApiServerConfiguration() : base("Api", 81)
        {
            if (Environment != Environment.Localhost)
            {
                UseRedisReplica = true;
            }
        }

        public override void Configure(IApplicationBuilder application, IHostEnvironment environment)
        {
            base.Configure(application, environment);
            application.UseServiceStack(new ApiServiceHost(this));
        }
    }
}