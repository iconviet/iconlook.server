using Agiper.Server;

namespace Iconlook.Service.Job
{
    public class JobServiceStack : ServiceStackBase
    {
        public JobServiceStack(HostConfiguration config) : base(config.HostName, typeof(JobServiceStack).Assembly, config)
        {
        }
    }
}