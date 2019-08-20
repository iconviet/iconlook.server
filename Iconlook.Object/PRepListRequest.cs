using Agiper.Object;
using ServiceStack;

namespace Iconlook.Object
{
    [Route("/v1.0/preps", "GET")]
    public class PrepListRequest : ListRequestBase<PrepListRequest, PrepResponse>, IGet
    {
        public string Edit { get; set; }
        public string Filter { get; set; }
    }
}