using Agiper.Object;
using ServiceStack;

namespace Iconlook.Object
{
    [Route("/v1/address/unstaking", "GET")]
    public class UnstakingAddressListRequest : ListRequestBase<UnstakingAddressListRequest, UnstakingAddressResponse>, IGet
    {
        public string Filter { get; set; }
    }
}