using Agiper.Object;
using ServiceStack;

namespace Iconlook.Object
{
    [Route("/v1/address/list", "GET")]
    public class AddressListRequest : ListRequestBase<AddressListRequest, AddressResponse>, IGet
    {
        public string State { get; set; }
    }
}