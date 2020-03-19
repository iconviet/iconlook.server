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
                var items = redis.As<UnstakingAddressResponse>().GetAll();
                switch (request.Filter)
                {
                    default:
                        items = items.Where(x => x.Unstaking >= 100).ToList();
                        break;
                    case "10000icx":
                        items = items.Where(x => x.Unstaking >= 10000).ToList();
                        break;
                    case "100000icx":
                        items = items.Where(x => x.Unstaking >= 100000).ToList();
                        break;
                    case "500000icx":
                        items = items.Where(x => x.Unstaking >= 500000).ToList();
                        break;
                    case "prep_only":
                        items = items.Where(x => x.Class == AddressClass.PRep).ToList();
                        break;
                }
                return new UnstakingAddressListResponse(items
                    .OrderByDescending(x => x.UnstakedBlockHeight)
                    .Skip(items.Count > request.Take ? request.Skip : 0).Take(request.Take))
                {
                    Count = items.Count,
                    Take = request.Take,
                    Skip = items.Count > request.Take ? request.Skip : 0
                };
            }
        }

        [CacheResponse(Duration = 60, LocalCache = true)]
        public object Any(UndelegatedAddressListRequest request)
        {
            using (var redis = Redis.Instance())
            {
                var items = redis.As<UndelegatedAddressResponse>().GetAll();
                switch (request.Filter)
                {
                    default:
                        items = items.Where(x => x.Staked >= 100).ToList();
                        break;
                    case "10000icx":
                        items = items.Where(x => x.Staked >= 10000).ToList();
                        break;
                    case "100000icx":
                        items = items.Where(x => x.Staked >= 100000).ToList();
                        break;
                    case "500000icx":
                        items = items.Where(x => x.Staked >= 500000).ToList();
                        break;
                }
                return new UndelegatedAddressListResponse(items
                    .OrderByDescending(x => x.Undelegated)
                    .Skip(items.Count > request.Take ? request.Skip : 0).Take(request.Take))
                {
                    Count = items.Count,
                    Take = request.Take,
                    Skip = items.Count > request.Take ? request.Skip : 0
                };
            }
        }
    }
}