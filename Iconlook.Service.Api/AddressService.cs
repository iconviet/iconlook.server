using System.Linq;
using Agiper.Server;
using Iconlook.Object;

namespace Iconlook.Service.Api
{
    public class AddressService : ServiceBase
    {
        public object Any(UnstakingAddressListRequest request)
        {
            using (var redis = Redis.Instance())
            {
                var addresses = redis.As<UnstakingAddressResponse>().GetAll().AsEnumerable();
                switch (request.Filter)
                {
                    case "due_today":
                        addresses = addresses.OrderBy(x => x.UnstakedBlockHeight);
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
                    case "prep_only":
                        addresses = addresses.Where(x => x.Class == AddressClass.PRep).OrderByDescending(x => x.UnstakedBlockHeight);
                        break;
                }
                return new UnstakingAddressListResponse(addresses.Skip(addresses.Count() > request.Take ? request.Skip : 0).Take(request.Take))
                {
                    Skip = request.Skip,
                    Take = request.Take,
                    Count = addresses.Count()
                };
            }
        }
    }
}