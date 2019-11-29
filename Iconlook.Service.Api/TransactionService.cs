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
            return new ListResponse<TransactionResponse>(Redis.As<TransactionResponse>().GetAll().OrderByDescending(x => x.Timestamp));
        }
    }
}