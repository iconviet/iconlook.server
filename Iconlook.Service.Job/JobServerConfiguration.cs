using Iconlook.Server;
using Microsoft.AspNetCore.Builder;
using ServiceStack;

namespace Iconlook.Service.Job
{
    public class JobServerConfiguration : HttpServerConfiguration
    {
        public JobServerConfiguration() : base("Job", 82)
        {
        }

        public override void Configure(IApplicationBuilder application)
        {
            base.Configure(application);
            application.UseServiceStack(new JobServiceHost(this));
        }
    }
}