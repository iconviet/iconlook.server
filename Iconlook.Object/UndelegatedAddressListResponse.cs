using System.Collections.Generic;
using Agiper.Object;

namespace Iconlook.Object
{
    public class UndelegatedAddressListResponse : ListResponse<UndelegatedAddressResponse>
    {
        public UndelegatedAddressListResponse()
        {
        }

        public UndelegatedAddressListResponse(IList<UndelegatedAddressResponse> items) : base(items)
        {
        }

        public UndelegatedAddressListResponse(IEnumerable<UndelegatedAddressResponse> items) : base(items)
        {
        }
    }
}