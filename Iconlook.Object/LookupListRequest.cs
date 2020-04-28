using Iconviet.Object;
using ServiceStack;

namespace Iconlook.Object
{
    [Route("/v1/lookup", "GET")]
    public class LookupListRequest : ListRequestBase<LookupListRequest, PRepResponse>, IGet
    {
        public string Filter { get; set; }
    }
}