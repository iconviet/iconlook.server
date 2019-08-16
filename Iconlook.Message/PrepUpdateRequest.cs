using Agiper.Object;
using ServiceStack;

namespace Iconlook.Message
{
    [Route("/v1.0/preps", "PUT")]
    public class PrepUpdateRequest : RequestBase<PrepUpdateRequest>, IGet
    {
        public int Score { get; set; }
        public string Id { get; set; }
        public string Location { get; set; }
    }
}