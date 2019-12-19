using System.Linq;
using Agiper.Object;
using Agiper.Server;
using Iconlook.Object;

namespace Iconlook.Service.Api
{
    public class PeerService : ServiceBase
    {
        public object Get(PeerListRequest request)
        {
            var redis = Redis.Instance();
            return new ListResponse<PeerResponse>(redis.As<PeerResponse>().GetAll().Take(request.Take).OrderBy(x => x.Rank));
        }
    }
}