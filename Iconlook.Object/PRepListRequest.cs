using Agiper.Object;
using ServiceStack;

namespace Iconlook.Object
{
    [Route("/v1.0/preps", "GET")]
    public class PRepListRequest : ListRequestBase<PRepListRequest, PRepResponse>, IGet
    {
        public string Edit { get; set; }
        public string Filter { get; set; }
    }
}