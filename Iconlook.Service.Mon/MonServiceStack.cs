using Iconviet.Server;

namespace Iconlook.Service.Mon
{
    public class MonServiceStack : ServiceStackBase
    {
        public MonServiceStack(HostConfiguration config) : base(config.ServiceName, typeof(MonServiceStack).Assembly, config)
        {
        }
    }
}