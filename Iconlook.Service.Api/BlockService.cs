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
            return new ListResponse<BlockResponse>(Redis.As<BlockResponse>().GetAll().Take(request.Take).OrderByDescending(x => x.Height));
        }
    }
}