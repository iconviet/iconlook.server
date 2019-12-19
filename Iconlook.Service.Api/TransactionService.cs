using System.Linq;
using Agiper.Object;
using Agiper.Server;
using Iconlook.Object;

namespace Iconlook.Service.Api
{
    public class TransactionService : ServiceBase
    {
        public object Any(TransactionListRequest request)
        {
            using (var redis = Redis.Instance())
            {
                var items = redis.As<TransactionResponse>().GetAll().OrderByDescending(x => x.Timestamp);
                return new ListResponse<TransactionResponse>(items.Skip(request.Skip).Take(request.Take));
            }
        }
    }
}