using Iconviet.Server;

namespace Iconlook.Service.Job
{
    public class JobServiceStack : ServiceStackBase
    {
        public JobServiceStack(HostConfiguration config) : base(config.ServiceName, typeof(JobServiceStack).Assembly, config)
        {
        }
    }
}