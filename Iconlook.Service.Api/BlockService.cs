using System.Linq;
using Agiper.Object;
using Agiper.Server;
using Iconlook.Object;

namespace Iconlook.Service.Api
{
    public class BlockService : ServiceBase
    {
        public object Any(BlockListRequest request)
        {
            using (var redis = Redis.Instance())
            {
                var items = redis.As<BlockResponse>().GetAll().OrderByDescending(x => x.Height);
                return new ListResponse<BlockResponse>(items.Skip(request.Skip).Take(request.Take));
            }
        }
    }
}