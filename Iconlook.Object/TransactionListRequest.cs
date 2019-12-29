using Agiper.Object;
using ServiceStack;

namespace Iconlook.Object
{
    [Route("/v1/transactions", "GET")]
    public class TransactionListRequest : ListRequestBase<TransactionListRequest, TransactionResponse>, IGet
    {
        public string Filter { get; set; }
    }
}