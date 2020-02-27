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
                    return new ListResponse<AddressResponse>(addresses.Skip(request.Skip).Take(request.Take))
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