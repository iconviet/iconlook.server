using System.Linq;
using Agiper.Server;
using Iconlook.Object;
using ServiceStack;

namespace Iconlook.Service.Api
{
    public class AddressService : ServiceBase
    {
        [CacheResponse(Duration = 60, LocalCache = true)]
        public object Any(UnstakingAddressListRequest request)
        {
            using (var redis = Redis.Instance())
            {
                var addresses = redis.As<UnstakingAddressResponse>().GetAll().AsEnumerable();
                switch (request.Filter)
                {
                    default:
                        addresses = addresses.Where(x => x.Unstaking >= 100).OrderByDescending(x => x.UnstakedBlockHeight);
                        break;
                    case "10000icx":
                        addresses = addresses.Where(x => x.Unstaking >= 10000).OrderByDescending(x => x.UnstakedBlockHeight);
                        break;
                    case "100000icx":
                        addresses = addresses.Where(x => x.Unstaking >= 100000).OrderByDescending(x => x.UnstakedBlockHeight);
                        break;
                    case "500000icx":
                        addresses = addresses.Where(x => x.Unstaking >= 500000).OrderByDescending(x => x.UnstakedBlockHeight);
                        break;
                    case "1000000icx":
                        addresses = addresses.Where(x => x.Unstaking >= 1000000).OrderByDescending(x => x.UnstakedBlockHeight);
                        break;
                    case "prep_only":
                        addresses = addresses.Where(x => x.Class == AddressClass.PRep).OrderByDescending(x => x.UnstakedBlockHeight);
                        break;
                }
                return new UnstakingAddressListResponse(addresses.Skip(addresses.Count() > request.Take ? request.Skip : 0).Take(request.Take))
                {
                    Take = request.Take,
                    Count = addresses.Count(),
                    Skip = addresses.Count() > request.Take ? request.Skip : 0
                };
            }
        }
    }
}