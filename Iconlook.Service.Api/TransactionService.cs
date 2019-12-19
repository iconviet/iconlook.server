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
            var redis = Redis.Instance();
            return new ListResponse<TransactionResponse>(redis.As<TransactionResponse>().GetAll().Take(request.Take).OrderByDescending(x => x.Timestamp));
        }
    }
}