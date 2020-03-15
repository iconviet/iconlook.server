using Agiper.Server;

namespace Iconlook.Service.Mon
{
    public class MonServiceStack : ServiceStackBase
    {
        public MonServiceStack(HostConfiguration config) : base(config.HostName, typeof(MonServiceStack).Assembly, config)
        {
        }
    }
}