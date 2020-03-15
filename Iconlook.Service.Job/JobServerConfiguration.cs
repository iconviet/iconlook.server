using Iconlook.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using ServiceStack;

namespace Iconlook.Service.Job
{
    public class JobServerConfiguration : HttpServerConfiguration
    {
        public JobServerConfiguration() : base("Job", 82)
        {
        }

        public override void Configure(IApplicationBuilder application, IHostEnvironment environment)
        {
            base.Configure(application, environment);
            application.UseServiceStack(new JobServiceHost(this));
        }
    }
}