using Agiper.Object;
using ServiceStack;

namespace Iconlook.Object
{
    [Route("/v1.0/transactions", "GET")]
    public class TransactionListRequest : ListRequestBase<TransactionListRequest, TransactionResponse>, IGet
    {
    }
}