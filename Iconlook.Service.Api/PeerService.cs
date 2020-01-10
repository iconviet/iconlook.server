using System.Linq;
using Agiper.Object;
using Agiper.Server;
using Iconlook.Object;
using ServiceStack;

namespace Iconlook.Service.Api
{
    public class PeerService : ServiceBase
    {
        [CacheResponse(Duration = 2, LocalCache = true)]
        public object Get(PeerListRequest request)
        {
            using (var redis = Redis.Instance())
            {
                var items = redis.As<PeerResponse>().GetAll()
                    .Where(x => x.State == "Vote").OrderBy(x => x.Ranking).ToList();
                return new ListResponse<PeerResponse>(items.Skip(request.Skip).Take(request.Take));
            }
        }
    }
}