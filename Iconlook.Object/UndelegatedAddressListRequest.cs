using Iconviet.Object;
using ServiceStack;

namespace Iconlook.Object
{
    [Route("/v1/address/list/undelegated", "GET")]
    public class UndelegatedAddressListRequest : ListRequestBase<UndelegatedAddressListRequest, UndelegatedAddressResponse>, IGet
    {
        public string Filter { get; set; }
    }
}