using Agiper.Object;
using ServiceStack;

namespace Iconlook.Message
{
    // TODO: switch route to /prep
    [Route("/v1.0/preps", "PUT")]
    public class PrepUpdateRequest : RequestBase<PrepUpdateRequest>, IGet
    {
        public int Score { get; set; }
        public string Id { get; set; }
        public string Location { get; set; }
    }
}