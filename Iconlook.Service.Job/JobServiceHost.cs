using Agiper.Server;

namespace Iconlook.Service.Job
{
    public class JobServiceHost : ServiceHostBase
    {
        public JobServiceHost(ServerConfiguration config) : base(config.HostName, typeof(JobServiceHost).Assembly, config)
        {
        }
    }
}