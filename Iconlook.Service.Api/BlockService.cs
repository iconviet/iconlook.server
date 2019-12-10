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
            return new ListResponse<BlockResponse>(Redis.As<BlockResponse>().GetAll().Take(13).OrderByDescending(x => x.Height));
        }
    }
}