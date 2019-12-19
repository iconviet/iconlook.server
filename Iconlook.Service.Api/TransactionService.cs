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
            var redis = Redis.Instance().As<TransactionResponse>();
            var items = redis.GetAll().OrderByDescending(x => x.Timestamp).ToList();
            return new ListResponse<TransactionResponse>(items.Skip(request.Skip).Take(request.Take));
        }
    }
}