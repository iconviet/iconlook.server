using Agiper.Object;
using Agiper.Server;
using Iconlook.Object;
using System.Linq;

namespace Iconlook.Service.Api
{
    public class BlockService : ServiceBase
    {
        public object Any(BlockListRequest request)
        {
            return new ListResponse<BlockResponse>(Redis.As<BlockResponse>().GetAll().OrderByDescending(x => x.Height));
        }
    }
}