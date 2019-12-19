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
            var redis = Redis.Instance().As<BlockResponse>();
            var items = redis.GetAll().OrderByDescending(x => x.Height);
            return new ListResponse<BlockResponse>(items.Skip(request.Skip).Take(request.Take));
        }
    }
}