using System.Collections.Generic;
using Agiper.Object;

namespace Iconlook.Object
{
    public class UnstakingAddressListResponse : ListResponse<AddressResponse>
    {
        public long BlockHeight { get; set; }

        public UnstakingAddressListResponse()
        {
        }

        public UnstakingAddressListResponse(IList<AddressResponse> items) : base(items)
        {
        }

        public UnstakingAddressListResponse(IEnumerable<AddressResponse> items) : base(items)
        {
        }
    }
}