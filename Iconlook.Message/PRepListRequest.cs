using Agiper.Object;
using ServiceStack;

namespace Iconlook.Message
{
    [Route("/v1.0/preps", "GET")]
    public class PrepListRequest : ListRequestBase<PrepListRequest, PrepResponse>, IGet
    {
    }
}