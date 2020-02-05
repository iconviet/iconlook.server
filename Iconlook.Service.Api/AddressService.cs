using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Agiper;
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
                using (var redis = Redis.Instance())
                {
                    var chainalytic = new ChainalyticClient();
                    var unstaking_info = await chainalytic.GetUnstakingInfo();
                    var prep_dictionary = redis.As<PRepResponse>().GetAll().ToDictionary(x => x.Address);
                    return new UnstakingAddressListResponse(
                        unstaking_info.GetWallets()
                            .Where(x => x.Value.Split(':').Length == 4 &&
                                        long.TryParse(x.Value.Split(':')[2], out _))
                            .Select(x =>
                            {
                                var tuple = x.Value.Split(':');
                                var name = prep_dictionary.TryGet(x.Key)?.Name;
                                return new AddressResponse
                                {
                                    Id = x.Key,
                                    Name = name,
                                    Hash = x.Key,
                                    UnstakedBlockHeight = long.Parse(tuple[3]),
                                    RequestedBlockHeight = long.Parse(tuple[2]),
                                    Description = name == null ? null : "ICON P-Rep",
                                    Staked = decimal.Parse(BigDecimal.Parse(tuple[0]).ToString()),
                                    Unstaking = decimal.Parse(BigDecimal.Parse(tuple[1]).ToString())
                                };
                            })
                            .OrderByDescending(x => x.RequestedBlockHeight).Skip(request.Skip).Take(request.Take));
                }
            }
            return new ListResponse<AddressResponse>();
        }
    }
}