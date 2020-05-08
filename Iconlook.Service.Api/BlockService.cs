using System.Linq;
using Iconlook.Object;
using Iconviet.Object;
using Iconviet.Server;

namespace Iconlook.Service.Api
{
    public class BlockService : ServiceBase
    {
        public object Any(BlockListRequest request)
        {
            using (var redis = Redis.Instance())
            {
                var blocks = redis.As<BlockResponse>().GetAll().OrderByDescending(x => x.Height);
                return new ListResponse<BlockResponse>(blocks.Skip(request.Skip).Take(request.Take));
            }
        }
    }
}