using System.Linq;
using Iconviet.Object;
using Iconviet.Server;
using Iconlook.Object;
using ServiceStack;

namespace Iconlook.Service.Api
{
    public class PeerService : ServiceBase
    {
        [CacheResponse(Duration = 20, LocalCache = true)]
        public object Get(PeerListRequest request)
        {
            using (var redis = Redis.Instance())
            {
                var peers = redis.As<PeerResponse>().GetAll()
                    .Where(x => x.State == "Vote" ||
                                x.State == "BlockGenerate").OrderBy(x => x.Ranking).ToList();
                return new ListResponse<PeerResponse>(peers.Skip(request.Skip).Take(request.Take));
            }
        }
    }
}