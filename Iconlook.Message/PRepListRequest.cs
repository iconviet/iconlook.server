using Agiper.Object;
using ServiceStack;

namespace Iconlook.Message
{
    [Route("/preps", "GET")]
    public class PRepListRequest : ListRequestBase<PRepListRequest, PRepResponse>, IGet
    {
    }
}