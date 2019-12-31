using Agiper.Server;
using ServiceStack;

namespace Iconlook.Service.Api
{
    public class StatusService : ServiceBase
    {
        [Route("/api/v1/status/host", "GET")]
        public class HostRequest : IReturn<string>, IGet
        {
        }

        public object Any(HostRequest _)
        {
            return new
            {
                status = "online"
            };
        }
    }
}