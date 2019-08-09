using Agiper.Object;
using ServiceStack;

namespace Iconlook.Message
{
    [Route("/preps", "GET")]
    public class PrepListRequest : ListRequestBase<PrepListRequest, PrepResponse>, IGet
    {
    }
}