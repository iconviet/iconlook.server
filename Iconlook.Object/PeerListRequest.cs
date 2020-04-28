using Iconviet.Object;
using ServiceStack;

namespace Iconlook.Object
{
    [Route("/v1/peers", "GET")]
    public class PeerListRequest : ListRequestBase<PeerListRequest, PeerResponse>, IGet
    {
    }
}