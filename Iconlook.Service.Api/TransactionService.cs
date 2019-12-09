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
            return new ListResponse<TransactionResponse>(Redis.As<TransactionResponse>().GetAll().Take(13).OrderByDescending(x => x.Timestamp));
        }
    }
}