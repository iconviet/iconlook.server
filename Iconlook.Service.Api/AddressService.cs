using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Agiper.Object;
using Agiper.Server;
using Iconlook.Client.Chainalytic;
using Iconlook.Object;
using ServiceStack;

namespace Iconlook.Service.Api
{
    public class AddressService : ServiceBase
    {
        [CacheResponse(Duration = 60, LocalCache = true)]
        public async Task<object> Any(AddressListRequest request)
        {
            if (request.State == "unstaking")
            {
                var chainalytic = new ChainalyticClient();
                var unstaking_info = await chainalytic.GetUnstakingInfo();
                return new UnstakingAddressListResponse(unstaking_info.GetWallets().Select(x =>
                {
                    var tuple = x.Value.Split(':');
                    return new AddressResponse
                    {
                        Id = x.Key,
                        Hash = x.Key,
                        UnstakedBlockHeight = long.Parse(tuple[2]),
                        Staked = decimal.Parse(BigDecimal.Parse(tuple[0]).ToString()),
                        Unstaking = decimal.Parse(BigDecimal.Parse(tuple[1]).ToString())
                    };
                })).ThenDo(x => x.BlockHeight = (long) unstaking_info.GetBlockHeight());
            }
            return new ListResponse<AddressResponse>();
        }
    }
}