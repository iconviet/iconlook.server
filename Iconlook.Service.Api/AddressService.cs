using System.Linq;
using Agiper.Object;
using Agiper.Server;
using Iconlook.Object;

namespace Iconlook.Service.Api
{
    public class AddressService : ServiceBase
    {
        public object Any(AddressListRequest request)
        {
            if (request.State == "unstaking")
            {
                using (var redis = Redis.Instance())
                {
                    var addresses = redis.As<AddressResponse>().GetAll().AsEnumerable();
                    switch (request.Param)
                    {
                        case "10000icx":
                            addresses = addresses.Where(x => x.Unstaking >= 10000);
                            break;
                        case "100000icx":
                            addresses = addresses.Where(x => x.Unstaking >= 100000);
                            break;
                        case "500000icx":
                            addresses = addresses.Where(x => x.Unstaking >= 500000);
                            break;
                        case "due_today":
                            addresses = addresses.OrderBy(x => x.UnstakedBlockHeight);
                            break;
                        case "prep_only":
                            addresses = addresses.Where(x => x.Class == AddressClass.PRep);
                            break;
                        case "last_24h":
                            addresses = addresses.OrderByDescending(x => x.UnstakedBlockHeight);
                            break;
                    }
                    return new ListResponse<AddressResponse>(addresses.Skip(request.Skip).Take(request.Take))
                    {
                        Skip = request.Skip,
                        Take = request.Take,
                        Count = addresses.Count()
                    };
                }
            }
            return new ListResponse<AddressResponse>();
        }
    }
}