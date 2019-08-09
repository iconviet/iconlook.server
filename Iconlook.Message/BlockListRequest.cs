using Agiper.Object;
using ServiceStack;

namespace Iconlook.Message
{
    [Route("/v1.0/blocks", "GET")]
    public class BlockListRequest : ListRequestBase<BlockListRequest, BlockResponse>, IGet
    {
    }
}