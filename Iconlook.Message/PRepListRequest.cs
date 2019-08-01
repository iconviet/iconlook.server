using Agiper.Object;
using ServiceStack;

namespace Iconlook.Message
{
    [Route("/api/preps", "GET")]
    public class PRepListRequest : ListRequestBase<PRepListRequest, PRepResponse>, IGet
    {
    }
}