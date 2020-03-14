using Agiper.Server;

namespace Iconlook.Service.Mon
{
    public class MonServiceHost : ServiceHostBase
    {
        public MonServiceHost(ServerConfiguration config) : base(config.HostName, typeof(MonServiceHost).Assembly, config)
        {
        }
    }
}