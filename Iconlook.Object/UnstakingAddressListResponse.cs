using System.Collections.Generic;
using Iconviet.Object;

namespace Iconlook.Object
{
    public class UnstakingAddressListResponse : ListResponse<UnstakingAddressResponse>
    {
        public UnstakingAddressListResponse()
        {
        }

        public UnstakingAddressListResponse(IList<UnstakingAddressResponse> items) : base(items)
        {
        }

        public UnstakingAddressListResponse(IEnumerable<UnstakingAddressResponse> items) : base(items)
        {
        }
    }
}