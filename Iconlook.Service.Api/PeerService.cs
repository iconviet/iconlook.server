using System.Linq;
using System.Threading.Tasks;
using Agiper.Object;
using Agiper.Server;
using Iconlook.Object;
using Iconlook.Service.Job;
using ServiceStack;

namespace Iconlook.Service.Api
{
    public class PeerService : ServiceBase
    {
        [CacheResponse(Duration = 2, LocalCache = true)]
        public async Task<object> Get(PeerListRequest request)
        {
            using (var redis = Redis.Instance())
            {
                var items = redis.As<PeerResponse>().GetAll().OrderBy(x => x.Ranking).ToList();
                if (!items.Any())
                {
                    await TryResolve<UpdatePeersJob>().RunAsync();
                    items = redis.As<PeerResponse>().GetAll().OrderBy(x => x.Ranking).ToList();
                }
                return new ListResponse<PeerResponse>(items.Skip(request.Skip).Take(request.Take));
            }
        }
    }
}