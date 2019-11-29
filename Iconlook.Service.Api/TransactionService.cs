using Agiper.Object;
using Agiper.Server;
using Iconlook.Object;
using System.Linq;

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