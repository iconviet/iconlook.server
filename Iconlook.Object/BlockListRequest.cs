using Iconviet.Object;
using ServiceStack;

namespace Iconlook.Object
{
    [Route("/v1/blocks", "GET")]
    public class BlockListRequest : ListRequestBase<BlockListRequest, BlockResponse>, IGet
    {
        public string Filter { get; set; }
    }
}