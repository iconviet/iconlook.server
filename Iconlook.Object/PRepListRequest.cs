using Iconviet.Object;
using ServiceStack;

namespace Iconlook.Object
{
    [Route("/v1/prep/list", "GET")]
    public class PRepListRequest : ListRequestBase<PRepListRequest, PRepResponse>, IGet
    {
        public string Edit { get; set; }
        public string Filter { get; set; }
    }
}