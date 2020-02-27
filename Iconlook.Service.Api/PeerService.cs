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