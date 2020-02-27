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
                    var addresses = redis.As<AddressResponse>().GetAll();
                    switch (request.Param)
                    {
                        case "4":
                            addresses = addresses.Where(x => x.Unstaking >= 10000).ToList();
                            break;
                        case "5":
                            addresses = addresses.Where(x => x.Unstaking >= 100000).ToList();
                            break;
                        case "6":
                            addresses = addresses.Where(x => x.Unstaking >= 500000).ToList();
                            break;
                    }
                    return new ListResponse<AddressResponse>(addresses.Skip(request.Skip).Take(request.Take).ToList())
                    {
                        Skip = request.Skip,
                        Take = request.Take,
                        Count = addresses.Count
                    };
                }
            }
            return new ListResponse<AddressResponse>();
        }
    }
}