using Agiper.Object;
using ServiceStack;

namespace Iconlook.Object
{
    [Route("/v1.0/peers", "GET")]
    public class PeerListRequest : ListRequestBase<PeerListRequest, PeerResponse>, IGet
    {
    }
}